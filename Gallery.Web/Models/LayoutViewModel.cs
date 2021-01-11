using Gallery.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Models
{
    public class LayoutViewModel
    {
        //Need to pass this model to _Layout
        public AppUser CurrentAppUser { get; set; }
    }
}
