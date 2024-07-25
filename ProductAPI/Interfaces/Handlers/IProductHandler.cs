using ProductAPI.Commands;
using ProductAPI.Models.Responses;

namespace ProductAPI.Interfaces.Handlers
{
    public interface IProductHandler
    {
        Task<ProductResponse?> HandleGetById(int id);
        Task<bool> HandleCreate(CreateProductCommand command);
        Task<bool> HandleUpdate(UpdateProductCommand command);
        Task<bool> HandleDelete(DeleteProductCommand command);
    }
}
