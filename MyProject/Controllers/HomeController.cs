using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.ViewModels;
using Project.Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<MyIdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger,UserManager<MyIdentityUser> _userManager)
        {
            _logger = logger;
            userManager = _userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            MyIdentityUser user = userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.Message = $"Welcome {user.FullName}!";
            List<ProductModel> pm = null;
            if (userManager.IsInRoleAsync(user, "NormalUser").Result)
            {
                ViewBag.RoleMessage = "You are an NormalUser.";

                // call to Web API // map to model
                pm = new CakeBakerHelper().GetProducts();
                return View(pm);
            }
            return View();
        }
        [HttpPost]
        public JsonResult ShoppingCart(string CakeId)
        {
            if (HttpContext.Session.GetInt32("CartCounter") != null)
            {
                HttpContext.Session.SetInt32("CartCounter", HttpContext.Session.GetInt32("CartCounter").Value + 1);
            } 
            else 
            {
                HttpContext.Session.SetInt32("CartCounter", 1);
            }

            var retjson = JsonConvert.SerializeObject(new { Success = true, Counter = HttpContext.Session.GetInt32("CartCounter").Value });
            return Json(data: retjson);
        }
        [Authorize(Roles = "admin")]

        public IActionResult OnlyAdminAccess()
        {
            ViewBag.role = "Admin";
            MyIdentityUser user = userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.Message = $"Welcome {user.FullName}!";
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
