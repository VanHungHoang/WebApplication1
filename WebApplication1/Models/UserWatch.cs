using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class UserWatch
    {
        public string UserId { get; set; }
        public int StoryId { get; set; }

        public virtual Stories Story { get; set; }
    }
}
