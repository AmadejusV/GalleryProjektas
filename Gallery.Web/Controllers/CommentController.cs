using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gallery.Data;
using Gallery.Domains;
using Gallery.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Gallery.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CommentController> _logger;

        public CommentController(Context context, UserManager<AppUser> userManager, ILogger<CommentController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Comment Index action visited at {time}", DateTime.Now);

            return View(await _context.Comments.ToListAsync());
        }

        //Comment details admin only entry
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid? id)
        {
            _logger.LogInformation("Comment Details action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id has null value");
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                _logger.LogError("comment object was not found or has null value");
                return NotFound();
            }

            return View(comment);
        }


        public  IActionResult Create(int? id)
        {
            _logger.LogInformation("Comment Create action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id has null value");
                return NotFound();
            }

            var commentViewModel = new CommentViewModel()
            {
                PostId=(int)id
            };

            return View(commentViewModel);
        }

        //Creating comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentViewModel commentModel)
        {
            if (ModelState.IsValid)
            {
                //getting current user
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);     
                AppUser applicationUser = await _userManager.FindByIdAsync(userId);

                var post = await _context.Posts.FirstOrDefaultAsync(m => m.PostId == commentModel.PostId);

                if (post == null)
                {
                    _logger.LogError("id has null value");
                    return NotFound();
                }

                var newComment = new Comment()
                {
                    Id = Guid.NewGuid(),
                    Text = commentModel.Text,
                    AppUser = applicationUser,
                    PostId = commentModel.PostId
                };

                //adding comment to comments table
                _context.Comments.Add(newComment);
                //adding comment to Post Comments list
                post.Comments.Add(newComment);

                _context.Update(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New comment created at {time}, id - {commentId}", DateTime.Now, newComment.Id);
                //Redirecting to a specific path
                return Redirect("~/Post/Details/" + commentModel.PostId); 
            }
            _logger.LogError("Model state is not valid");
            return View(commentModel);
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            _logger.LogInformation("Comment Edit action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id has null value");
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                _logger.LogError("comment object was not found or has null value");
                return NotFound();
            }
            return View(comment);
        }


        // Editting specific comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Comment comment)
        {
            if (id != comment.Id)
            {
                _logger.LogError("id doesn't match model Id");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        _logger.LogError("Comment does not exist");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.LogInformation("Comment updated at {time},id - {commentId}", DateTime.Now, comment.Id);
                return Redirect("~/Post/Details/" + comment.PostId);
            }
            _logger.LogError("Model state is not valid, cannot update");
            return View(comment);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            _logger.LogInformation("Comment Delete action visited at {time}", DateTime.Now);

            if (id == null)
            {
                _logger.LogError("id has null value");
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                _logger.LogError("Object was not found or has null value");
                return NotFound();
            }
            return View(comment);
        }


        // Deleting specific comment
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Comment deleted at {time}, id - {commentId}", DateTime.Now, comment.Id);
            return Redirect("~/Post/Details/" + comment.PostId);
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
