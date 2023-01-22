using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _db;

        public ProductRepository(ProductContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetByNameAsync(string name)
        {
            return await _db.Products.Where(c => c.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Product>> GetByPriceRange(decimal from, decimal to)
        {
            return await _db.Products.Where(c => c.Price >= from && c.Price <= to).ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
    }
}
