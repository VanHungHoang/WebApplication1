using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int AuthorStatus { get; set; }
        //public string Slug { get; set; }
    }
}
