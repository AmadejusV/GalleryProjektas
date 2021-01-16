using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Domains;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Web.Controllers
{
    public class SharedController : Controller
    {

        private readonly UserManager<AppUser> _userManager;

        public SharedController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //[ChildActionOnly] ?   //Tried to pass model to _Layout View
        public async Task<IActionResult> Header()
        {

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser currentAppUser = await _userManager.FindByIdAsync(userId);

            var currentAppUserModel = new LayoutViewModel()
            {
                CurrentAppUser=currentAppUser
            };

            return View(currentAppUserModel);
        }
    }
}
