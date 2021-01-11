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
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PostController> _logger;

        public PostController(Context context, IWebHostEnvironment hostEnvironment, UserManager<AppUser> userManager, ILogger<PostController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Post Index action visited at {time}", DateTime.Now);

            return View(await _context.Posts.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> GuestGallery()
        {
            _logger.LogInformation("Post GuestGallery action visited at {time}", DateTime.Now);

            return View(await _context.Posts.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("Post Details action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id has null value");
                return NotFound();
            }

            //ThenInclude helps include an object property that belongs to Comments list object
            var post = await _context.Posts.Include(c => c.Comments).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                _logger.LogError("post has null value");
                return NotFound();
            }

            //finding currentAppUser by Id using
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
            return View(detailsViewModel);
        }


        public IActionResult Create()
        {
            _logger.LogInformation("Post Create action visited at {time}", DateTime.Now);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                //sets path to wwwroot folder
                string wwwRootPath = _hostEnvironment.WebRootPath;                      
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string extension = Path.GetExtension(post.ImageFile.FileName);
                //creating unique name for the new image
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //setting ImageName prop to be used for displaying image
                post.ImageName = fileName;                                             
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                var newPostObject = new Post()
                {
                    ImageName = post.ImageName,
                    Title = post.Title,
                    Details = post.Details,
                    ImageFile = post.ImageFile
                };

                //creating/uploading the new image in the set path
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await post.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(newPostObject);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New post created at {time}, postId - {postId}", DateTime.Now, newPostObject.PostId);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Model is not valid, could not create new post");
            return View(post);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("Post Edit action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id is null");
                return NotFound();
            }

            //Can't include ImageFile therefore during edit need to reselect the image
            var post = await _context.Posts./*Include(p => p.ImageFile).*/FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                _logger.LogError("post could not be found or is null");
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
                _logger.LogError("id does not match post id or is null");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string extension = Path.GetExtension(post.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //newPostName used to set the new ImageName after deleting the old one
                var newPostName = fileName;
                var newPath = Path.Combine(wwwRootPath + "/Image/", fileName);

                if (newPath != null)
                {
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await post.ImageFile.CopyToAsync(fileStream);
                    }

                    var oldPostObject = await _context.Posts.FindAsync(id);
                    var oldPath = Path.Combine(_hostEnvironment.WebRootPath, "Image", oldPostObject.ImageName);

                    //Deletes old image file
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
                            _logger.LogError("post does not exist with id - {postId}", post.PostId);
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                _logger.LogInformation("post edited successfully at {time}, id - {postId}", DateTime.Now, post.PostId);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Model is not valid couldn't update");
            return View(post);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("Post Delete action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id is null");
                return NotFound();
            }

            var image = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (image == null)
            {
                _logger.LogError("image could not be found or is null");
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

            _logger.LogInformation("Post has been deleted successfully at {time}, id - {postId}", DateTime.Now, post.PostId);
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
