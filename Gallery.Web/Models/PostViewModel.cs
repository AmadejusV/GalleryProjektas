using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        [Required(ErrorMessage = "A title is required")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Enter a title for your image")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image generated name")]
        public string ImageName { get; set; }
        [StringLength(255, ErrorMessage = "Details must not exceed 255 characters length")]
        public string Details { get; set; }
        [NotMapped]
        [DisplayName("Upload an image")]
        [Required(ErrorMessage = "Must choose an image to be displayed")]
        public IFormFile ImageFile { get; set; }
    }
}
