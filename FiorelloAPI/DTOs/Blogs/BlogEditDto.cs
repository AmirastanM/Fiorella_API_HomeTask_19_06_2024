using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Blogs
{
    public class BlogEditDto
    {
       
        public string Title { get; set; }       
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
