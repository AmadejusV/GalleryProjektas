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

namespace Gallery.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(Context context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Comments.ToListAsync());
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }


        public  IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentViewModel = new CommentViewModel()
            {
                PostId=(int)id
            };

            return View(commentViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentViewModel commentModel)
        {
            if (ModelState.IsValid)
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);        //getting current user
                AppUser applicationUser = await _userManager.FindByIdAsync(userId);

                var post = await _context.Posts.FirstOrDefaultAsync(m => m.PostId == commentModel.PostId);

                if (post == null)
                {
                    return NotFound();
                }

                var newComment = new Comment()
                {
                    Id = Guid.NewGuid(),
                    Text = commentModel.Text,
                    AppUser = applicationUser,
                    PostId = commentModel.PostId
                };

                _context.Comments.Add(newComment);  //adding comment to comments table
                post.Comments.Add(newComment);      //adding comment to Post Comments list

                _context.Update(post);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect("~/Post/Details/" + commentModel.PostId); //Redirecting to a specific path
            }
            return View(commentModel);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, /*[Bind("Id,Text,TimeCreated")]*/ Comment comment)
        {
            if (id != comment.Id)
            {
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
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/Post/Details/" + comment.PostId);
            }
            return View(comment);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Redirect("~/Post/Details/" + comment.PostId);
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
