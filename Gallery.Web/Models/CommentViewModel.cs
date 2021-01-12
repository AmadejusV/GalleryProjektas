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
        [Required]
        public string Text { get; set; }
        public int PostId { get; set; }
    }
}
