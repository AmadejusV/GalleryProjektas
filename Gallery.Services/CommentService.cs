using Gallery.Data;
using Gallery.Domains;
using Gallery.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Services
{
    public class CommentService : ICommentService
    {
        private readonly Context _context;
        private readonly IPostService _postService;
        private readonly ILogger<CommentService> _logger;
        public CommentService(Context context, IPostService postService, ILogger<CommentService> logger)
        {
            _context = context;
            _postService = postService;
            _logger = logger;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(Guid? id)
        {
            return await _context.Comments.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateCommentAsync(string text, AppUser appUser, int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {   //questionable if needed?
                _logger.LogError("id has null value");
            }

            var newGuidId = Guid.NewGuid();
            _context.Comments.Add(new Comment()
            {
                Id = newGuidId,
                Text = text,
                AppUser = appUser,
                PostId = postId
            });
            _context.Update(post);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New comment created at {time}, id - {commentId}", DateTime.Now, newGuidId);
        }

        public async Task EditCommentByIdAsync(Comment comment)
        {
            _context.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentByIdAsync(Guid id)
        {
            _context.Comments.Remove(await GetCommentByIdAsync(id));
            await _context.SaveChangesAsync();
            _logger.LogInformation("Comment deleted at {time}, id - {commentId}", DateTime.Now, id);
        }

        public bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
