using Gallery.Data;
using Gallery.Domains;
using Gallery.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Services
{
    public class PostService : IPostService
    {
        private readonly Context _context;
        public PostService(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int? id)
        {
            return await _context.Posts.Include(c => c.Comments).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.PostId == id);
        }

        public async Task CreatePostAsync(string imageName, string title, string details, IFormFile imageFile)
        {
            _context.Posts.Add(new Post()
            {
                ImageName = imageName,
                Title = title,
                Details = details,
                ImageFile = imageFile
            });
            await _context.SaveChangesAsync();
        }

        public async Task EditPostByIdAsync(int id, string imageName, string title, string details, IFormFile imageFile)
        {
            var post = await GetPostByIdAsync(id);
            post.ImageName = imageName;
            post.Title = title;
            post.Details = details;
            post.ImageFile = imageFile;
            
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostByIdAsync(int id)
        {
            _context.Posts.Remove(await GetPostByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public bool ImageExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
