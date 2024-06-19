using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Experts
{
    public class ExpertEditDto
    {     
        public string FullName { get; set; }
        public string Position { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
