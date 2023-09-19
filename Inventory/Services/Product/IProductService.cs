namespace Inventory.Services.Product
{
    public interface IProductService
    {
        public Task<IEnumerable<Entities.Product>> Get();
        public Task<Entities.Product> GetOne(int id);
    }
}
