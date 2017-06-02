using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Authors
    {
        public Authors()
        {
            Stories = new HashSet<Stories>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorStatus { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Stories> Stories { get; set; }
    }
}
