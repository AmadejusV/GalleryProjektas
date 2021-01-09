using Gallery.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Models
{
    public class DetailsViewModel
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string Details { get; set; }
        public List<Comment> Comments { get; set; }
        public int PostId { get; set; }
        public AppUser CurrentAppUser { get; set; }

    }
}

