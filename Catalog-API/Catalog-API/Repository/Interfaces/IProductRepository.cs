using Catalog_API.Models;

namespace Catalog_API.Repository.Interfaces;

public interface IProductRepository: IRepository<Product>
{
    IEnumerable<Product> ListProductsByPrice();
}
