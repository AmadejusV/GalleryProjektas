using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery.Domains
{
    public class AppUser : IdentityUser<Guid> //added Guid type
    {
        public string Name { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
