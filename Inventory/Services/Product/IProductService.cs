using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Inventory.Services.Product
{
    public interface IProductService
    {
        public Task<IEnumerable<Entities.Product>> Get();
        public Task<Entities.Product> GetOne(int id);

        public Task<IEnumerable<TEntity>> LoadAllWithRelatedAsync<TEntity>(params Expression<Func<TEntity, object>>[] expressionList) where TEntity : class;
        public void SaveTableRecord<TEntity>(List<DynamicField> fields, Dictionary<string, object> fieldValues) where TEntity : class;
    }

}
