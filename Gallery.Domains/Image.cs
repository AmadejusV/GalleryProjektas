using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery.Domains
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Details { get; set; }

    }
}
