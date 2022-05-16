using Catalog_API.Context;
using Catalog_API.Models;
using Catalog_API.Pagination;
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

    public PagedList<Product> GetProducts(ProductParameters parameters)
    {
        return PagedList<Product>.ToPagedList(Get().OrderBy(p => p.ProductId),
            parameters.PageNumber,parameters.PageSize);
    }
}
