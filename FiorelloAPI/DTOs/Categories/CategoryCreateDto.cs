using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Categories
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(50, ErrorMessage = "The name must be at most 50 characters long")]
        public string Name { get; set; }
    }
}
