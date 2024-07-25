using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Interfaces.Handlers;
using ProductAPI.Interfaces.Services;

namespace ProductAPI.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductHandler _productHandler;

        public ProductController(
            ILogger<ProductController> logger, 
            ICacheService cacheService, 
            IProductHandler productHandler)
        {
            _logger = logger;
            _productHandler = productHandler;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productHandler.HandleGetById(id);
            if (product is null)
            {
                return NotFound();
            }

            return Json(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productHandler.HandleCreate(command);
            return Json("OK");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _productHandler.HandleUpdate(command);
            if (success)
            {
                return Json("OK");
            }
            return Json("No update was done");
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var success = await _productHandler.HandleDelete(new DeleteProductCommand { ProductId = productId });
            if (success)
            {
                return Json("OK");
            }
            return Json("No product was deleted");
        }

    }
}
