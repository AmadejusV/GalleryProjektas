using Gallery.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(Guid? id);
        Task CreateCommentAsync(string text, AppUser appUser, int postId);
        Task EditCommentByIdAsync(Comment comment);
        Task DeleteCommentByIdAsync(Guid id);
        bool CommentExists(Guid id);
    }
}
