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
        public DbSet<Image> Images { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
