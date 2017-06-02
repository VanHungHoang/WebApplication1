using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Stories
    {
        public Stories()
        {
            Chapters = new HashSet<Chapters>();
            StoryGenre = new HashSet<StoryGenre>();
            UserWatch = new HashSet<UserWatch>();
        }

        public int StoryId { get; set; }
        public string StoryName { get; set; }
        public int StoryProgress { get; set; }
        public string StoryDescription { get; set; }
        public int StoryStatus { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int RateCount { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Chapters> Chapters { get; set; }
        public virtual ICollection<StoryGenre> StoryGenre { get; set; }
        public virtual ICollection<UserWatch> UserWatch { get; set; }
        public virtual Authors Author { get; set; }
    }
}
