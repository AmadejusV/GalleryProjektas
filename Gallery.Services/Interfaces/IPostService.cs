using Gallery.Domains;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int? id);
        Task CreatePostAsync(string imageName, string title, string details, IFormFile imageFile);
        Task EditPostByIdAsync(int id, string imageName, string title, string details, IFormFile imageFile);
        Task DeletePostByIdAsync(int id);
        bool ImageExists(int id);
    }
}
