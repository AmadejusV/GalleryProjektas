using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Models
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(16)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
