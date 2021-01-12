using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Domains;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RegistrationController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger<RegistrationController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        //Allows anonymous visits to the index view
        [AllowAnonymous]
        public IActionResult Index()
        {
            _logger.LogInformation("Registration Index action visited at {time}", DateTime.Now);
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogInformation("Registration Login action visited at {time}", DateTime.Now);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AuthError = "The email adress must be valid, the password must contain atleast 1 capital letter, number, symbol and cannot be shorter than 6 symbols long.";

                _logger.LogError("Cannot login, model state is not valid");
                return View("Login", model);
            }

            //user sign in attempt
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {user} signed in at {time}", model.Email, DateTime.Now);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AuthError = "Sumtin Wong! Are you a registered user? If so check if entered data is correct.";
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
            _logger.LogInformation("Registration Register action visited at {time}", DateTime.Now);
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Comments = new List<Comment>()
                };

                var result = await _userManager.CreateAsync(newUser, model.ConfirmPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {user} registered at {time}", newUser.Email, DateTime.Now);
                    return RedirectToAction("Login");
                }
            }
            _logger.LogError("Cannot register model state is not valid");
            return View("~/Views/Registration/Index.cshtml", model);
        }
    }
}
