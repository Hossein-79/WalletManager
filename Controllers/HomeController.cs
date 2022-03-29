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
using WalletManager.Models.ViewModel;
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
        private readonly ICoinPriceService _coinPriceService;

        public HomeController(ILogger<HomeController> logger, ICoinPriceService coinPriceService, IUserService userService, ICovalentService covalentService, IChainService chainService, IWalletAddressService walletAddressService)
        {
            _logger = logger;
            _userService = userService;
            _covalentService = covalentService;
            _chainService = chainService;
            _walletAddressService = walletAddressService;
            _coinPriceService = coinPriceService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            return View();
        }

        public async Task<IActionResult> Login(string name, string password)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction(nameof(Dashboard));
            //}

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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddressBalance(string address, string chainId)
        {
            var balances = await _covalentService.GetAddressBalance(address, chainId);

            foreach (var item in balances)
            {
                item.CoinPrice = await _coinPriceService.GetCoinPrice(item.Symbol);

                if (item.CoinPrice is null)
                {
                    var price = await _covalentService.GetPrice(item.Symbol);

                    if (price != 0)
                    {
                        var coinPrice = new CoinPrice
                        {
                            Symbol = item.Symbol,
                            Price = price,
                            ContractAddress = item.ContractAddress,
                            LastUpdateTime = DateTime.UtcNow,
                        };
                        item.CoinPrice = coinPrice;
                        await _coinPriceService.Add(coinPrice);
                    }
                }
            }

            return Json(new { Success = true, Message = new { balances, Total = balances.Where(u => u.CoinPrice != null).Sum(u => u.Value * u.CoinPrice.Price) } });
        }

        public async Task<IActionResult> AddressBalancePartial(string address, string chainId)
        {
            var balances = await _covalentService.GetAddressBalance(address, chainId);

            foreach (var item in balances)
            {
                item.CoinPrice = await _coinPriceService.GetCoinPrice(item.Symbol);

                if (item.CoinPrice is null)
                {
                    var price = await _covalentService.GetPrice(item.Symbol);

                    if (price != 0)
                    {
                        var coinPrice = new CoinPrice
                        {
                            Symbol = item.Symbol,
                            Price = price,
                            ContractAddress = item.ContractAddress,
                            LastUpdateTime = DateTime.UtcNow,
                        };
                        item.CoinPrice = coinPrice;
                        await _coinPriceService.Add(coinPrice);
                    }
                }
            }

            return PartialView("_AddressBalancePartial", new { Success = true, Message = balances });
        }

        [Authorize]
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
                Label = lable,
            };

            await _walletAddressService.Add(walletAddress);

            var balances = await _covalentService.GetAddressBalance(address, chain.CovalentId);
            return Json(new { Success = true, Message = balances });
        }

        public async Task<IActionResult> GetAllChains(bool update = false)
        {
            // Get Chains from database
            var chains = await _chainService.GetAllChains();

            if (update || chains is null)
            {
                //Get Chains from API
                var allChains = await _covalentService.GetAllChain();
                foreach (var item in allChains)
                {
                    if (!chains.Contains(item))
                    {
                        await _chainService.Add(item);
                        chains.Append(item);
                    }
                }
            }

            return Json(new { Success = true, Message = chains });
        }

        public async Task<IActionResult> WalletPreviewPartial(string address, string chainId)
        {
            var balances = await _covalentService.GetAddressBalance(address, chainId);

            if (balances is null)
                return PartialView("_WalletPreviewPartial");

            foreach (var item in balances)
            {
                item.CoinPrice = await _coinPriceService.GetCoinPrice(item.Symbol);

                if (item.CoinPrice is null)
                {
                    var price = await _covalentService.GetPrice(item.Symbol);

                    if (price != 0)
                    {
                        var coinPrice = new CoinPrice
                        {
                            Symbol = item.Symbol,
                            Price = price,
                            ContractAddress = item.ContractAddress,
                            LastUpdateTime = DateTime.UtcNow,
                        };
                        item.CoinPrice = coinPrice;
                        await _coinPriceService.Add(coinPrice);
                    }
                }
            }

            var model = new WalletPreviewViewModel()
            {
                Balances = balances.OrderBy(u => u.Value * u.CoinPrice.Price).Take(3),
                CoinCount = balances.Count(),
                Total = balances.Where(u => u.CoinPrice != null).Sum(u => u.Value * u.CoinPrice.Price)
            };

            return PartialView("_WalletPreviewPartial", model);
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var model = new DashboardViewModel()
            {
                Addresses = await _walletAddressService.GetUserWallets(user.UserId),
                Chains = await _chainService.GetAllChains(),
            };

            return View(model);
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
