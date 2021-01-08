using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Domains;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Web.Controllers
{
    public class RegistrationController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;  //patikrins programa
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistrationController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AuthError = "Sumtin Wong! Are you a registered user? If so check if entered data is correct.";
                return View("Login", model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Comments = new List<Comment>()
                };

                var result = await _userManager.CreateAsync(newUser, model.ConfirmPassword);
                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Login");
                }
            }
            return View("~/Views/Registration/Index.cshtml", model);
        }
    }
}
