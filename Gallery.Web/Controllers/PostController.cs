using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gallery.Data;
using Gallery.Domains;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Gallery.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Gallery.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public PostController(Context context, IWebHostEnvironment hostEnvironment, UserManager<AppUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> GuestGallery()
        {
            return View(await _context.Posts.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(c => c.Comments).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.PostId == id);
            //ThenInclude helps include an object property that belongs to Comments list object

            if (post == null)
            {
                return NotFound();
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser currentAppUser = await _userManager.FindByIdAsync(userId);

            var detailsViewModel = new DetailsViewModel()
            {
                Comments = post.Comments,
                PostId = post.PostId,
                ImageName = post.ImageName,
                Details = post.Details,
                Title = post.Title,
                CurrentAppUser = currentAppUser
            };
            // perduot detailsViewModel ir palygint commentaru appuserius su current appuseriu, gal reikes hidden fieldu laikyt informacijai
            return View(detailsViewModel);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;                      //sets path to wwwroot folder
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string extension = Path.GetExtension(post.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;   //creating unique name for the new image
                post.ImageName = fileName;                                             //setting ImageName prop to be used for displaying image
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                var newPostObject = new Post()
                {
                    ImageName = post.ImageName,
                    Title = post.Title,
                    Details = post.Details,
                    ImageFile = post.ImageFile
                };

                using (var fileStream = new FileStream(path, FileMode.Create))           //creating/uploading the new image in the set path
                {
                    await post.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(newPostObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts./*Include(p => p.ImageFile).*/FirstOrDefaultAsync(p => p.PostId == id);
            //Can't include ImageFile therefore during edit need to reselect the image

            if (post == null)
            {
                return NotFound();
            }
            var postViewModel = new PostViewModel()
            {
                PostId = post.PostId,
                Title = post.Title,
                Details = post.Details,
                ImageName = post.ImageName,
                ImageFile = post.ImageFile
            };

            return View(postViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostViewModel post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string extension = Path.GetExtension(post.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                var newPostName = fileName;                        //used to set the new ImageName after deleting the old one
                var newPath = Path.Combine(wwwRootPath + "/Image/", fileName);

                if (newPath != null)
                {
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await post.ImageFile.CopyToAsync(fileStream);
                    }

                    var oldPostObject = await _context.Posts.FindAsync(id);
                    var oldPath = Path.Combine(_hostEnvironment.WebRootPath, "Image", oldPostObject.ImageName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    };


                    oldPostObject.ImageName = newPostName;
                    oldPostObject.Details = post.Details;
                    oldPostObject.Title = post.Title;

                    try
                    {
                        _context.Posts.Update(oldPostObject);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ImageExists(post.PostId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", post.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            };

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
