using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class StoryGenre
    {
        public int StoryId { get; set; }
        public int GenreId { get; set; }

        public virtual Genres Genre { get; set; }
        public virtual Stories Story { get; set; }
    }
}
