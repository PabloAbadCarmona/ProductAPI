using ProductAPI.Interfaces.Repositories;
using ProductAPI.Models;
using ProductAPI.Persistence;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<Product?> GetProduct(int productId)
        => JsonStorage.ReadOneWithFilterAsync<Product>(x => x.ProductId == productId && x.DeletedAt == null);

        public Task<bool> AddProduct(Product product)
        { 
            product.CreatedAt = DateTime.Now;
            return JsonStorage.AddAsync(product, x => x.ProductId == product.ProductId);
        }

        public Task<bool> UpdateProduct(Product product)
        => JsonStorage.UpdateAsync(product, x => x.ProductId == product.ProductId);

        public async Task<bool> DeleteProduct(int productId) 
        {
            var productToDelete = await GetProduct(productId);
            if (productToDelete is not null)
            {
                productToDelete.DeletedAt = DateTime.Now;
                return await UpdateProduct(productToDelete);
            }
            return false;
        }

        public Dictionary<int, string> GetProductStatuses()
        => new Dictionary<int, string> { { 0, "Inactive" } , { 1, "Active" } };
        
    }
}
