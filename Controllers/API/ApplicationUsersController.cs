using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses;
using MVCIdentityBookRecords.Responses.Users;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="SuperAdmin,Admin")]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUsersController> _logger;
        private readonly IEmailSender _emailSender;

        public ApplicationUsersController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ApplicationUsersController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        //TODO: CREATE UserDTO => Return only specific fields to frontend e.g username, firstname, lastname, email, phonenumber
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var response = users.ConvertAll(o => new UserResponse
            {
                UserId = o.Id,
                Username = o.UserName,
                Email = o.Email,
                Firstname = o.FirstName,
                Lastname = o.LastName,
                PhoneNumber = o.PhoneNumber,
            });

            return response;
        }

        // GET: api/Users/e6acdb85-adff-439c-9b71-862f1b065962
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponse>> GetUser(string id)
        {
            var user = await _userManager.Users.Where(_ => _.Id == id)
                .Include(_=> _.Books)
                .ThenInclude(_ => _.Categories)
                .ThenInclude(_ => _.Books)
                .ThenInclude(_ => _.Authors)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            var response = new UserDetailResponse()
            {
                Iduser = user.Id,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber, 
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Books = user.Books,
            };

            return response;
        }
        // PUT: api/Users/e6acdb85-adff-439c-9b71-862f1b065962
        [HttpPut("{id}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string id, UpdateUserRequest userData) 
        {
            //find user from usermanager
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to find user with ID '{_userManager.GetUserId(User)}'.");
            }

            var firstName = user.FirstName;
            var lastName = user.LastName;
            
            
            if (userData.FirstName != firstName && userData.FirstName != null)
            {
                user.FirstName = userData.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (userData.LastName != lastName && userData.LastName != null)
            {
                user.LastName = userData.LastName;
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (userData.PhoneNumber != phoneNumber && userData.PhoneNumber != null)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, userData.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return BadRequest("Unexpected error when trying to set phone number.");
                }
            }

            //Handle picture upload
            //if (Request.Form.Files.Count > 0)
            //{
            //    IFormFile file = Request.Form.Files.FirstOrDefault();
            //    using (var dataStream = new MemoryStream())
            //    {
            //        await file.CopyToAsync(dataStream);
            //        user.ProfilePicture = dataStream.ToArray();
            //    }
            //    await _userManager.UpdateAsync(user);
            //}
            if (user.UsernameChangeLimit > 0 && userData.UserName != null)
            {
                if (userData.UserName != user.UserName)
                {
                    var userNameExists = await _userManager.FindByNameAsync(userData.UserName);
                    if (userNameExists != null)
                    {
                        //StatusMessage = "User name already taken. Select a different username.";
                        return BadRequest("User name already taken. Select a different username.");
                    }
                    var setUserName = await _userManager.SetUserNameAsync(user, userData.UserName);
                    if (!setUserName.Succeeded)
                    {
                        //StatusMessage = "Unexpected error when trying to set user name.";
                        return BadRequest("Unexpected error when trying to set user name.");
                    }
                    else
                    {
                        user.UsernameChangeLimit -= 1;
                        await _userManager.UpdateAsync(user);
                    }
                }
            }
            return Content("User Updated.");
        }

        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> CreateUserAsync([FromBody]RegisterRequest model) {

            if (model.Password != model.ConfirmPassword)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RegisterResponse { Success = false, Error = "Password do not match" });
            }
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Success = false, Error = "User already exists!" });

            ApplicationUser user = new()
            {
                UserName = model.Username,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Success = false, Error = "User creation failed! Please check user details and try again." });
            //Add basic role to new user
            await _userManager.AddToRoleAsync(user, UserRoles.Basic);
            return Ok(new RegisterResponse { Success = true, Email = user.Email, Username = user.UserName });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return BadRequest("User not found");
            }
            var deleteResponse = await _userManager.DeleteAsync(user);
            if (deleteResponse.Succeeded)
            {
                return Ok("User deleted");
            }
            return BadRequest(deleteResponse.Errors);
        }


        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("This requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
