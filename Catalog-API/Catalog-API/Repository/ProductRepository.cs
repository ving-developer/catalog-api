using Catalog_API.Context;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog_API.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogApiContext context) : base(context){}

    /**
     * Returns all products sorted by price
     */
    public async Task<IEnumerable<Product>> ListProductsByPrice()
    {
        return await Get().OrderBy(c => c.Price).ToListAsync();
    }

    public PagedList<Product> GetProducts(ProductParameters parameters)
    {
        return PagedList<Product>.ToPagedList(Get().OrderBy(p => p.ProductId),
            parameters.PageNumber,parameters.PageSize);
    }
}
