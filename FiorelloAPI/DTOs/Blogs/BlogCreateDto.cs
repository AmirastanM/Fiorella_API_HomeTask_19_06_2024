using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Blogs
{
    public class BlogCreateDto
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(20, ErrorMessage = "The name must be at most 20 characters long")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(100, ErrorMessage = "The name must be at most 100 characters long")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This input can't be empty")]       
        public IFormFile Image { get; set; }
    }
}
