using Gallery.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery.Data
{
    public class Context : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)       //calling base constructor
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //Admin Seed 
            Guid ADMIN_ID = Guid.NewGuid();
            Guid ADMIN_ROLE = Guid.NewGuid();

            //seed admin role

            builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = ADMIN_ROLE,
                ConcurrencyStamp = "27747190-7b7d-453d-ba7b-5bfa31119160"
            });


            //create user
            var appUser = new AppUser
            {
                Id = ADMIN_ID,
                Email = "igne@admin.com",
                NormalizedEmail = "IGNE@ADMIN.COM",
                EmailConfirmed = true,
                UserName = "igne@admin.com",
                NormalizedUserName = "IGNE@ADMIN.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEAlgwj8nK0YJlLN9vWhPrDweK0wc2Nh/299BrJGh6zlBkTfF5oNO8dBW6Xjd+vAgJA==",
                ConcurrencyStamp = "2753310e-b58f-419e-bcb5-50d619ef4ed2",
                Name = "Adminas",
                SecurityStamp = "2S3BIYUGVFUZY4FBPDZZ4354ZFCRBVUV"
            };

            //seed user
            builder.Entity<AppUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ADMIN_ROLE,
                UserId = ADMIN_ID
            });
        }
    }
}
