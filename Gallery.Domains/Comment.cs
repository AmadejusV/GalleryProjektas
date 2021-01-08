using System;

namespace Gallery.Domains
{
    public class Comment
    {
        public Guid Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Text { get; set; }
        public DateTime TimeCreated { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public Comment()
        {
            TimeCreated = DateTime.Now;
        }
    }
}