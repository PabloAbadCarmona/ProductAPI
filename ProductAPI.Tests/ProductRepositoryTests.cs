using ProductAPI.Interfaces.Repositories;
using ProductAPI.Models;
using ProductAPI.Persistence;
using ProductAPI.Repositories;

namespace ProductAPI.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private IProductRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new ProductRepository();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            JsonStorage.DeleteFile<Product>();
        }

        [TestMethod]
        public async Task AddProduct_ShouldAdd()
        {
            var product = new Product
            {
                ProductId = 666,
                Name = "Keyboard",
                Description = "HyperX Alloy FPS",
                Stock = 10,
                CreatedAt = DateTime.Now,
                Price = 110,
                Status = 1
            };

            await _repository.AddProduct(product);
            var addedProduct = await _repository.GetProduct(product.ProductId);
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual(product.Name, addedProduct.Name);
            Assert.AreEqual(product.ProductId, addedProduct.ProductId);
            Assert.AreEqual(product.Description, addedProduct.Description);
            Assert.AreEqual(product.Stock, addedProduct.Stock);
            Assert.AreEqual(product.CreatedAt, addedProduct.CreatedAt);
            Assert.AreEqual(product.Price, addedProduct.Price);
        }

        [TestMethod]
        public async Task UpdateProduct_ShouldUpdate()
        {
            var product = new Product
            {
                ProductId = 1,
                Name = "Keyboard",
                Description = "HyperX Alloy FPS",
                Stock = 10,
                CreatedAt = DateTime.Now,
                Price = 110,
                Status = 1
            };

            await _repository.AddProduct(product);

            product = new Product
            {
                ProductId = 1,
                Name = "Keyboard",
                Description = "HyperX Alloy FPS",
                Stock = 9,
                CreatedAt = DateTime.Now,
                Price = 100,
                Status = 1  
            };

            await _repository.UpdateProduct(product);
            var updated = await _repository.GetProduct(product.ProductId);
            Assert.IsNotNull(updated);
            Assert.AreEqual(product.Name, updated.Name);
            Assert.AreEqual(product.ProductId, updated.ProductId);
            Assert.AreEqual(product.Description, updated.Description);
            Assert.AreEqual(product.Stock, updated.Stock);
            Assert.AreEqual(product.CreatedAt, updated.CreatedAt);
            Assert.AreEqual(product.Price, updated.Price);
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldDelete()
        {
            var product = new Product
            {
                ProductId = 1,
                Name = "Keyboard",
                Description = "HyperX Alloy FPS",
                Stock = 10,
                CreatedAt = DateTime.Now,
                Price = 110,
                Status = 1
            };

            await _repository.AddProduct(product);
            var productId = product.ProductId;

            var added = await _repository.GetProduct(productId);   
            Assert.IsNotNull(added);
            await _repository.DeleteProduct(productId);
            var deleted = await _repository.GetProduct(productId);
            Assert.IsNull(deleted);
        }
    }
}