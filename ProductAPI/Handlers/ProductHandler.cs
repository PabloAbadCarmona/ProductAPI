using ProductAPI.Commands;
using ProductAPI.Interfaces.Handlers;
using ProductAPI.Interfaces.Repositories;
using ProductAPI.Interfaces.Services;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
using Serilog;

namespace ProductAPI.Handlers
{
    public class ProductHandler : IProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IDiscountService _discountService;
        public ProductHandler(IProductRepository productRepository, ICacheService cacheService, IDiscountService discountService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _discountService = discountService;
        }

        public async Task<ProductResponse?> HandleGetById(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                if (product is not null)
                {
                    var statusName = _cacheService.GetStatusName(product.Status);
                    if (statusName is null)
                    {
                        var statuses = _productRepository.GetProductStatuses();
                        statuses.TryGetValue(product.Status, out statusName);
                        _cacheService.SetProductStatusDictionary(statuses);
                    }
                    var productResponse = new ProductResponse(product);
                    productResponse.StatusName = statusName;
                    var discount = await _discountService.GetDiscount(product.ProductId);
                    productResponse.Discount = discount;
                    return productResponse;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }                   
    
            return null;
        }

        public Task<bool> HandleCreate(CreateProductCommand command)
        {
            var product = new Product
            {
                ProductId = command.ProductId,
                Description = command.Description,
                Name = command.Name,
                Price = command.Price,
                Status = command.Status,
                Stock = command.Stock,
                CreatedAt = DateTime.UtcNow,
            };

            try
            {
                return _productRepository.AddProduct(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> HandleUpdate(UpdateProductCommand command)
        {
            var existingProduct = await _productRepository.GetProduct(command.ProductId);
            if (existingProduct != null)
            {
                var product = new Product
                {
                    ProductId = command.ProductId,
                    Description = string.IsNullOrEmpty(command.Description) ? existingProduct.Description : command.Description,
                    Name = string.IsNullOrEmpty(command.Name) ? existingProduct.Name : command.Name,
                    Price = command.Price is null ? existingProduct.Price : command.Price.Value,
                    Status = command.Status is null ? existingProduct.Status : command.Status.Value,
                    Stock = command.Stock is null ? existingProduct.Stock : command.Stock.Value,
                };
                try
                { 
                    await _productRepository.UpdateProduct(product);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return false;
                }
            }
            return false;
        }

        public Task<bool> HandleDelete(DeleteProductCommand command)
        => _productRepository.DeleteProduct(command.ProductId);
    }
}
