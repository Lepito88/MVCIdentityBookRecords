using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        // GET: api/Users/e6acdb85-adff-439c-9b71-862f1b065962
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _userManager.Users.Where(_ => _.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        // PUT: api/Users/e6acdb85-adff-439c-9b71-862f1b065962
        [HttpPut("{id}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string id, ApplicationUser userData) 
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
                    //StatusMessage = "Unexpected error when trying to set phone number.";
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
