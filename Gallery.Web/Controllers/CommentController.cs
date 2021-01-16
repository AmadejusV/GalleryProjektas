using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gallery.Domains;
using Gallery.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Gallery.Services.Interfaces;

namespace Gallery.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, IPostService postService, UserManager<AppUser> userManager, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _userManager = userManager;
            _logger = logger;
            _postService = postService;
        }


        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Comment Index action visited at {time}", DateTime.Now);

            return View(await _commentService.GetAllCommentsAsync());
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

            var comment = await _commentService.GetCommentByIdAsync(id);
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
                PostId = (int)id
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

                //adding comment to comments table
                //adding comment to Post Comments list
                await _commentService.CreateCommentAsync(commentModel.Text, applicationUser, commentModel.PostId);

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

            var comment = await _commentService.GetCommentByIdAsync(id);
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
                    await _commentService.EditCommentByIdAsync(comment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_commentService.CommentExists(comment.Id))
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

            var comment = await _commentService.GetCommentByIdAsync(id);
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
            var commentObject = await _commentService.GetCommentByIdAsync(id);
            await _commentService.DeleteCommentByIdAsync(id);

            return Redirect("~/Post/Details/" + commentObject.PostId);
        }
    }
}
