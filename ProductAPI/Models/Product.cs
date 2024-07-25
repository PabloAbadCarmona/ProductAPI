namespace ProductAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}