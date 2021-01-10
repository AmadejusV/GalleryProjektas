using System;
using System.ComponentModel;

namespace Gallery.Domains
{
    public class Comment
    {
        public Guid Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Text { get; set; }
        [DisplayName("Time of composing")]
        public DateTime TimeCreated { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public Comment()
        {
            TimeCreated = DateTime.Now;
        }
    }
}