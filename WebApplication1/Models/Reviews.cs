using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Reviews
    {
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewContent { get; set; }
        public int ReviewStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int RateCount { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
    }
}
