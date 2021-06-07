using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.AspNetCore.Identity;
using Project.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
            private readonly UserManager<MyIdentityUser> userManager;
            private readonly SignInManager<MyIdentityUser> loginManager;
            private readonly RoleManager<MyIdentityRole> roleManager;

            public AccountController(UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> loginManager, RoleManager<MyIdentityRole> roleManager)
            {
                this.userManager = userManager;
                this.loginManager = loginManager;
                this.roleManager = roleManager;
               AddAdmin();
            }

        private void AddAdmin()
        {
            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                MyIdentityRole role = new MyIdentityRole();
                role.Name = "admin";
                role.Description = "administer web site";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Error while creating role!");
                }
                else
                {
                    MyIdentityUser user = new MyIdentityUser();
                    user.UserName = "admin";
                    user.Email = "admin@test.com";
                    user.FullName = "site admin";

                    IdentityResult result = userManager.CreateAsync(user, "test123").Result;
                    userManager.AddToRoleAsync(user, "admin").Wait();
                }


            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //user creation
        //role creation
        //assign role to user

        public IActionResult Register(RegisterViewModel obj)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = new MyIdentityUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                user.FullName = obj.FullName;
                user.BirthDate = obj.Birthdate;

                IdentityResult result = userManager.CreateAsync(user, obj.Password).Result; //create user
                if (result.Succeeded)
                { //assign role to user
                    if (!roleManager.RoleExistsAsync("NormalUser").Result) //role exist
                    { //if no then create
                        MyIdentityRole role = new MyIdentityRole();
                        role.Name = "NormalUser";
                        role.Description = "Perform normal operations.";
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result; //create role
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Error while creating role!");
                            return View(obj);
                        }
                    }
                    userManager.AddToRoleAsync(user, "NormalUser").Wait(); //assign role to the user
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    IEnumerable<IdentityError> errors = result.Errors;
                    foreach (var item in errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {

                var result = loginManager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, true).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); //valid user
                }
                else
                {
                    ModelState.AddModelError("validmsg", "Invalid login!");//invalid username or password
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("locked", "Account is locked");//more then 3 wrong password attempt
                }
                //  ModelState.AddModelError("validmsg", "Invalid login!"); //invalid username or password
            }
            return View(obj); //failing server side validation
        }

        public IActionResult LogOff()
        {
            loginManager.SignOutAsync().Wait();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }


}
