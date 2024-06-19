using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Experts
{
    public class ExpertCreateDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Position { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

