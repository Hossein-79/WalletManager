using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IWalletAddressService _walletAddressService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, ICovalentService covalentService, IChainService chainService, IWalletAddressService walletAddressService)
        {
            _logger = logger;
            _userService = userService;
            _covalentService = covalentService;
            _chainService = chainService;
            _walletAddressService = walletAddressService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string name, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction(nameof(Wallet));
            }

            var user = await _userService.GetUser(name);

            if (name == null || password == null)
            {
                return Json(new { Success = false, Message = "Please enter username and password." });
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
                return Json(new { Success = false, Message = "incorrect password" });
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

            return Json(new { Success = true, Message = "Login Successful" });
        }

        public async Task<IActionResult> AddressBalance(string address, string chainId)
        {
            var balances = await _covalentService.GetAddressBalance(address, chainId);
            return Json(new { Success = true, Message = balances });
        }

        public async Task<IActionResult> AddAddress(string address, int chainId, string lable)
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var chain = await _chainService.GetChain(chainId);

            if (user is null)
                return Json(new { Success = false, Message = "User not found." });

            if (chain is null)
                return Json(new { Success = false, Message = "blockChain not found." });

            var walletAddress = new WalletAddress()
            {
                Address = address,
                UserId = user.UserId,
                ChainId = chain.ChainId,
                Lable = lable,
            };

            await _walletAddressService.Add(walletAddress);

            var balances = await _covalentService.GetAddressBalance(address, chain.CovalentId);
            return Json(new { Success = true, Message = balances });
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
            return Json(new { Success = true, Message = chains });
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var address = await _walletAddressService.GetUserWallets(user.UserId);

            return View(address);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
