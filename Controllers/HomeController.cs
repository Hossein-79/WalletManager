using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletManager.Helpers;
using WalletManager.Models;
using WalletManager.Services;

namespace WalletManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ICovalentService _covalentService;
        private readonly IChainService _chainService;


        public HomeController(ILogger<HomeController> logger, IUserService userService, ICovalentService covalentService, IChainService chainService)
        {
            _logger = logger;
            _userService = userService;
            _covalentService = covalentService;
            _chainService = chainService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction(nameof(Wallet));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction(nameof(Wallet));
            }

            var user = await _userService.GetUser(name);

            if (name == null || password == null)
            {
                ViewBag.Errors = "Please enter username and password.";
                return View();
            }

            if (user == null)
            {
                user = new User()
                {
                    Name = name,
                    Password = password.Sha256(),
                };
                await _userService.Add(user);
            }
            else if (user.Password != password.Sha256())
            {
                ViewBag.Errors = "incorrect password";
                return View();
            }

            var identity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, user.Name),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            });

            return Json(true);
        }

        public IActionResult Test1()
        {
            return View();
        }

        public IActionResult Test2()
        {
            return View();
        }

        public IActionResult Test3()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetAllChains()
        {
            // Get Chains from API
            //var chains = await _covalentService.GetAllChain();
            //foreach (var item in chains)
            //{
            //    var chain = await _chainService.GetChainByCovalentId(item.CovalentId);
            //    if (chain is null)
            //    {
            //        await _chainService.Add(item);
            //    }
            //}

            // Get Chains from database
            var chains = await _chainService.GetAllChains();
            return Json(chains);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
