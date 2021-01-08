using Gallery.Domains;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Models
{
    public class CommentViewModel
    {
        //public Guid Id { get; set; }
        //public DateTime TimeCreated { get; set; }
        //public string Title { get; set; }
        //public string ImageName { get; set; }
        //public string Details { get; set; }
        //[NotMapped]
        //public IFormFile ImageFile { get; set; }
        //public List<Comment> Comments { get; set; }  //beprasmis
        [Required]
        public string Text { get; set; }    //beprasmis
        //public AppUser AppUser { get; set; }
        public int PostId { get; set; }
        
        //public Post Post { get; set; }

    }
}
