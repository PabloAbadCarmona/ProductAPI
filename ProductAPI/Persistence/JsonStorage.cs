using ProductAPI.Exceptions;
using System.Text.Json;

namespace ProductAPI.Persistence
{
    public class JsonStorage
    {
        public static async Task<bool> AddAsync<T>(T entity, Func<T, bool> filter)
        {
            var file = $"{typeof(T)}.json";
            if (File.Exists(file))
            {
                var data = await File.ReadAllTextAsync(file);
                var products = JsonSerializer.Deserialize<IEnumerable<T>>(data) ?? [];
                var productExists = products.FirstOrDefault(filter) is not null;
                if (productExists)
                {
                    throw new StorageIntegrityException("Item with the same Id already exists");
                }
                products = products.Append(entity);
                data = JsonSerializer.Serialize(products);
                await File.WriteAllTextAsync(file, data);
            }
            else
            {
                var data = JsonSerializer.Serialize(new List<T> { entity });
                await File.WriteAllTextAsync(file, data);
            }
            return true;
        }

        public static async Task<IEnumerable<T>> ReadAllAsync<T>()
        {
            var file = $"{typeof(T)}.json";
            if (File.Exists(file))
            {
                var data = await File.ReadAllTextAsync(file);
                return JsonSerializer.Deserialize<IEnumerable<T>>(data) ?? [];
            }
            return new List<T>();
        }

        public static async Task<T?> ReadOneWithFilterAsync<T>(Func<T, bool> filter)
        {
            var file = $"{typeof(T)}.json";
            if (File.Exists(file))
            {
                var data = await File.ReadAllTextAsync(file);
                var products = JsonSerializer.Deserialize<IEnumerable<T>>(data);
                if (products is not null)
                {
                    return products.FirstOrDefault(filter);
                }
            }
            return default;
        }

        public static async Task<bool> UpdateAsync<T>(T entity, Func<T, bool> filter)
        {
            var file = $"{typeof(T)}.json";
            if (File.Exists(file))
            {
                var data = await File.ReadAllTextAsync(file);
                var products = JsonSerializer.Deserialize<IEnumerable<T>>(data) ?? [];
                var productToUpdate = products.FirstOrDefault(filter);
                if (productToUpdate is not null)
                {
                    products = products.Where(x => !productToUpdate.Equals(x));
                    products = products.Append(entity);
                    data = JsonSerializer.Serialize(products);
                    await File.WriteAllTextAsync(file, data);
                    return true;
                }
            }

            return false;
        }

        public static void DeleteFile<T>()
        {
            var file = $"{typeof(T)}.json";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}
