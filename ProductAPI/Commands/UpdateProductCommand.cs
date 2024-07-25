namespace ProductAPI.Commands
{
    public class UpdateProductCommand
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public int? Stock { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
    }
}
