namespace FiorelloAPI.DTOs.Categories
{
    public class CategoryProductDto
    {
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
        public ICollection<string> Products { get; set; }
        public int ProductCount { get; set; }
    }
}
