namespace ProductAPI.Interfaces.Services
{
    public interface IDiscountService
    {
        Task <double> GetDiscount(int productId);
    }
}
