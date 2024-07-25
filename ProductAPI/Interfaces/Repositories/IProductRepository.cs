using ProductAPI.Models;

namespace ProductAPI.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProduct(int productId);
        Task<bool> AddProduct(Product product);        
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        Dictionary<int, string> GetProductStatuses();
    }
}
