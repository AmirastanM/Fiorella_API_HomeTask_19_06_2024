namespace FiorelloAPI.DTOs.Products
{
    public class ProductImageEditDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
    }
}
