using FiorelloAPI.Models;

namespace FiorelloAPI.DTOs.Products
{
    public class ProductDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<ProductImageDto> Images { get; set; }
    }
}
