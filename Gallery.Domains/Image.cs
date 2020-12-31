using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Gallery.Domains
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string Details { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
