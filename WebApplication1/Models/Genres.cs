using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Genres
    {
        public Genres()
        {
            StoryGenre = new HashSet<StoryGenre>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int GenreStatus { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<StoryGenre> StoryGenre { get; set; }
    }
}
