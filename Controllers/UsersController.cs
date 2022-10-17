using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Controllers.API;
using MVCIdentityBookRecords.Enums.Roles;
using MVCIdentityBookRecords.Models;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text;
using MVCIdentityBookRecords.Requests;

namespace MVCIdentityBookRecords.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUsersController> _logger;
        //private readonly IEmailSender _emailSender;
        
        public string ReturnUrl { get; set; } = "";
        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string UserNameChangeLimitMessage { get; set; }

        public UsersController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
           // IUserEmailStore<ApplicationUser> emailStore,
            //IEmailSender emailSender,
            ILogger<ApplicationUsersController> logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            //_emailStore = emailStore ?? throw new ArgumentNullException(nameof(emailStore));
            //_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: UsersController
        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }

        // GET: UsersController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userManager.Users.Where(_ => _.Id == id)
                .Include(_ => _.Books)
                .ThenInclude(_ => _.Categories)
                .ThenInclude(_ => _.Books)
                .ThenInclude(_ => _.Authors)
                .FirstOrDefaultAsync();
            
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Username,Email,Password,,ConfirmPassword,Firstname,Lastname")] RegisterRequest request)
        {
            ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
               
                MailAddress address = new MailAddress(request.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = request.Email,
                    FirstName = request.Firstname,
                    LastName = request.Lastname
                };
                //var user = CreateUser();

                await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
                //await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, UserRoles.Basic);

                    //var userId = await _userManager.GetUserIdAsync(user);

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //  "/Account/ConfirmEmail",
                    //  pageHandler: null,
                    //  values: new { area = "Identity", userId = userId, code = code, returnUrl = ReturnUrl },
                    //  protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //  await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                    //}
                    try
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        return View();
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();

           
        }

        // GET: UsersController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind("UserName, FirstName, LastName,PhoneNumber,ProfilePicture")]ApplicationUser editRequest)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var firstName = user.FirstName;
            var lastName = user.LastName;

            if (editRequest.FirstName != firstName)
            {
                user.FirstName = editRequest.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (editRequest.LastName != lastName)
            {
                user.LastName = editRequest.LastName;
                await _userManager.UpdateAsync(user);
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (editRequest.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, editRequest.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return View(StatusMessage);
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }
            if (user.UsernameChangeLimit > 0)
            {
                if (editRequest.UserName != user.UserName)
                {
                    var userNameExists = await _userManager.FindByNameAsync(editRequest.UserName);
                    if (userNameExists != null)
                    {
                        StatusMessage = "User name already taken. Select a different username.";
                        //return RedirectToAction(nameof(Index));
                        return View(StatusMessage);
                    }
                    var setUserName = await _userManager.SetUserNameAsync(user, editRequest.UserName);
                    if (!setUserName.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set user name.";
                        //return RedirectToAction(nameof(Index));
                        return View(StatusMessage);
                    }
                    else
                    {
                        user.UsernameChangeLimit -= 1;
                        await _userManager.UpdateAsync(user);
                    }
                }
            }

            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));

        }

        // GET: UsersController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View();
            }
            try
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
