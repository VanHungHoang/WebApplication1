using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Chapters
    {
        public int ChapterId { get; set; }
        public int StoryId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string ChapterContent { get; set; }
        public int ChapterStatus { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public string UserId { get; set; }
        public string Slug { get; set; }

        public virtual Stories Story { get; set; }
    }
}
