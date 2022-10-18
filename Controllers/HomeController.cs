using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace MVCIdentityBookRecords.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEntityRelationShipManagerService _entityRelationShipManagerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public HomeController(ILogger<HomeController> logger,
            IEntityRelationShipManagerService entityRelationShipManagerService,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
        {
            _logger = logger;
            _entityRelationShipManagerService = entityRelationShipManagerService;
            _userManager = userManager;
            _userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> MyBooks()
        {
            var UserId= HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            
            //Get user by Id and include tables
            var user = await _userManager.Users
                .Where(user => user.Id == UserId.Value)
                .Include(_ => _.Books)
                .ThenInclude(_ => _.Authors)
                .ThenInclude(_ => _.Books)
                .ThenInclude(_ => _.Categories)
                .SingleOrDefaultAsync();
            if (user == null)
                return View();
           
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}