using Catalog_API.Models;
using Catalog_API.Pagination;

namespace Catalog_API.Repository.Interfaces;

public interface IProductRepository: IRepository<Product>
{
    Task<IEnumerable<Product>> ListProductsByPriceAsync();

    Task<PagedList<Product>> GetProductsAsync(ProductParameters parameters);
}
