using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProductAPI.Commands;
using ProductAPI.Controllers;
using ProductAPI.Interfaces.Handlers;
using ProductAPI.Interfaces.Services;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
using Serilog;

namespace ProductAPI.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private ProductController _controller;
        private ServiceProvider _serviceProvider;
        private Mock<IProductHandler> _productHandlerMock;
        private Mock<ICacheService> _cacheServiceMock;
        private ILogger<ProductController> _logger;

        [TestInitialize]
        public void Setup()
        {
            _productHandlerMock = new Mock<IProductHandler>();
            _cacheServiceMock = new Mock<ICacheService>();

            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddSerilog())
                .BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger<ProductController>();

            _controller = new ProductController(_logger, _cacheServiceMock.Object, _productHandlerMock.Object);
        }

        [TestMethod]
        public async Task CreateProduct_ShouldReturnJsonResult()
        {
            var command = new CreateProductCommand
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100
            };

            _productHandlerMock.Setup(x => x.HandleCreate(It.IsAny<CreateProductCommand>()))
                .Returns(Task.FromResult(true));

            var result = await _controller.AddProduct(command);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
        }

        [TestMethod]
        public async Task GetProduct_ShouldReturnJsonResult()
        {
            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Description = "Test Description",
                Status = 0,
                Price = 1,
                Stock = 10
            };

            _productHandlerMock.Setup(x => x.HandleGetById(It.IsAny<int>()))
                .Returns(Task.FromResult(new ProductResponse(product)));

            var result = await _controller.GetProduct(1);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
        }

        [TestMethod]
        public async Task GetNullProduct_ShouldReturnNotFound()
        {
            ProductResponse? productResponse = null;

            _productHandlerMock.Setup(x => x.HandleGetById(It.IsAny<int>()))
                .Returns(Task.FromResult(productResponse));
            var result = await _controller.GetProduct(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
