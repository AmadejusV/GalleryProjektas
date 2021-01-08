using Gallery.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery.Data
{
    public class Context : IdentityDbContext<AppUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)       //calling base constructor
        {

        }
    }
}
