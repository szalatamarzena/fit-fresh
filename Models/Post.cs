using System;
using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class Post
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime PostDate { get; set; }
        [Required]
        public string Content { get; set; }
        public String ImageMimeType { get; set; }
        public byte[] Image { get; set; }
    }
}