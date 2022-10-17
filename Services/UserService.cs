using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using MVCIdentityBookRecords.Controllers.API;
using Microsoft.AspNetCore.WebUtilities;
using MVCIdentityBookRecords.Enums.Roles;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses;

namespace MVCIdentityBookRecords.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUsersController> _logger;
        private readonly IEmailSender _emailSender;

        public UserService(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> emailStore,
            ILogger<ApplicationUsersController> logger,
            IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = emailStore;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<CreateUserResponse> CreateUserAsync(RegisterRequest request)
        {
            if (request != null)
            {

                MailAddress address = new MailAddress(request.Email);

                string userName = address.User;
                var newUser = new ApplicationUser
                {
                    UserName = userName,
                    Email = request.Email,
                    FirstName = request.Firstname,
                    LastName = request.Lastname
                };
                //var user = CreateUser();

                await _userStore.SetUserNameAsync(newUser, newUser.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(newUser, newUser.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(newUser, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Basic);

                    var userId = await _userManager.GetUserIdAsync(newUser);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    return new CreateUserResponse { Success = true, Iduser = userId, Username = newUser.UserName, Email = newUser.Email };
                }

                return new CreateUserResponse { Success = false, Error = "Unable to create user", ErrorCode = "" };

                //    //user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                //    await _context.Users.AddAsync(user);
                //    var createResponse = await _context.SaveChangesAsync();
                //    if (createResponse >= 0)
                //    {
                //        return new CreateUserResponse
                //        {
                //            Success = true,
                //            Iduser = user.Id,
                //            Email = user.Email,
                //            Username = user.UserName
                //        };
                //    }
                //    return new CreateUserResponse
                //    {
                //        Success = false,
                //        Error = "Unable to save user",
                //        ErrorCode = "U04"
                //    };
                //}

                //return new CreateUserResponse
                //{
                //    Success = false,
                //    Error = "Unable to create user",
                //    ErrorCode = "U05"
                //};

            }
            return new CreateUserResponse { Success = false, Error = "No information given", ErrorCode = "" };
        }
        public async Task<CreateUserResponse> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Error = "ApplicationUser Not Found",
                    ErrorCode = "U02"
                };
            }
           
            var deleteResponse = await _userManager.DeleteAsync(user);
            if (deleteResponse != null)
            {
                return new CreateUserResponse
                {
                    Success = true,
                    Email = user.Email,
                    Username = user.UserName
                };
            }

            return new CreateUserResponse
            {
                Success = false,
                Error = "Unable to delete user",
                ErrorCode = "U03"

            };
        }

        public async Task<UserDetailResponse> GetUserByIdAsync(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(_ => _.Books)
                .ThenInclude(_ => _.Authors)
                .ThenInclude(_=> _.Books)
                .ThenInclude(_ => _.Categories)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return new UserDetailResponse
                {
                    Success = false,
                    Error = "ApplicationUser not found",
                    ErrorCode = "U02",
                };
            }

            return new UserDetailResponse
            {
                Success = true,
                Iduser = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Books = user.Books
            };
        }

        public async Task<GetUsersResponse> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0)
            {
                return new GetUsersResponse
                {
                    Success = false,
                    Error = "No Users found",
                    ErrorCode = "U01"
                };
            }
            return new GetUsersResponse { Success = true, Users = users };
        }

        public async Task<CreateUserResponse> UpdateUserAsync(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Error = "User ids Don't match",
                    ErrorCode = "U06"

                };
            }
            //if (user.Password != null)
            //{
            //    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //}

            //user.UpdatedAt = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return new CreateUserResponse
                    {
                        Success = false,
                        Error = "Author Not found",
                        ErrorCode = "U02",
                    };
                }
                else
                {
                    throw;
                }
            }

            return new CreateUserResponse
            {
                Success = true,
                Iduser = user.Id,
                Email = user.Email,
                Username = user.UserName,
                
            };
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
//U01 : No users found
//U02 : User not found
//U03 : Unable to delete user
//U04 : Unable to save user
//U05 : Unable to create user
//U96 : User ids Don't match