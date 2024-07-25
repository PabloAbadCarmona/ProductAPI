namespace ProductAPI.Interfaces.Services
{
    public interface ICacheService
    {
        void SetProductStatusDictionary(Dictionary<int, string> statusDictionary);
        Dictionary<int, string>? GetProductStatusDictionary();
        string? GetStatusName(int statusKey);
    }
}
