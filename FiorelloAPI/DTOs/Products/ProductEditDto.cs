using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.DTOs.Products
{
    public class ProductEditDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        public int CategoryId { get; set; }
        public List<ProductImageEditDto> Images { get; set; }
        public List<IFormFile> NewImages { get; set; }

    }
}
