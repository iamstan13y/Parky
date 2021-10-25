using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger,
            ITrailRepository trailRepo,
            INationalParkRepository npRepo,
            IAccountRepository accountRepo)
        {
            _logger = logger;
            _trailRepo = trailRepo;
            _npRepo = npRepo;
            _accountRepo = accountRepo;

        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel listOfParksAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailAPIPath)
            };
            return View(listOfParksAndTrails);
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

        [HttpGet]
        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            User userObj = await _accountRepo.LoginAsync(SD.AccountAPIPath + "authenticate/", user); 
            
            if(userObj.Token == null)
            {
                return View();
            }

            HttpContext.Session.SetString("JWToken", userObj.Token);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            bool result = await _accountRepo.RegisterAsync(SD.AccountAPIPath + "register/", user);

            if (result== false)
            {
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }
    }
}
