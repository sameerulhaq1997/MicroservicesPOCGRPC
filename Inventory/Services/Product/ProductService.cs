using Inventory.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;
        public ProductService(InventoryContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Entities.Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Entities.Product> GetOne(int id)
        {
            var isExist = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(isExist == null)
            {
                return new Entities.Product();
            }
            return isExist;
        }

        public async Task<int> Add(Entities.Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return product.Id;
        }
    }
}
