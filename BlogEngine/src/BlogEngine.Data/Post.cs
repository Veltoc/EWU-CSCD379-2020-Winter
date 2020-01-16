using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Data
{
    public class Post : FingerPrintEntityBase
    {
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public List<Comment> Comments { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public string Slug { get; set; }
    }
}
