namespace FiorelloAPI.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedDate { get; set; }
        public ICollection<string> Products { get; set; }
        public int ProductCount { get; set; }
    }
}
