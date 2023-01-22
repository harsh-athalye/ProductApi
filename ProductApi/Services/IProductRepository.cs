using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);

        Task<List<Product>> GetByNameAsync(string name);

        Task<List<Product>> GetByPriceRange(decimal from, decimal to);

        Task AddAsync(Product product);
        
        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}
