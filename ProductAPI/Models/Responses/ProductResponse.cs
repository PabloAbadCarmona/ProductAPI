namespace ProductAPI.Models.Responses
{
    public class ProductResponse
    {
        public ProductResponse(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Stock = product.Stock;
            Description = product.Description;
            Price = product.Price;
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? StatusName { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double FinalPrice
        {
            get
            {
                return Price * (100 - Discount) / 100;
            }
        }
    }
}
