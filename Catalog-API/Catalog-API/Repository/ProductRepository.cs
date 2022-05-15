using Catalog_API.Context;
using Catalog_API.Models;
using Catalog_API.Repository.Interfaces;

namespace Catalog_API.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogApiContext context) : base(context){}

    /**
     * Returns all products sorted by price
     */
    public IEnumerable<Product> ListProductsByPrice()
    {
        return Get().OrderBy(c => c.Price).ToList();
    }
}
