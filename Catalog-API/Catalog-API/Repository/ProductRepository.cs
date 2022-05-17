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
    public async Task<IEnumerable<Product>> ListProductsByPriceAsync()
    {
        return await Get().OrderBy(c => c.Price).ToListAsync();
    }

    public async Task<PagedList<Product>> GetProductsAsync(ProductParameters parameters)
    {
        return await PagedList<Product>.ToPagedListAsync(Get().OrderBy(p => p.ProductId),
            parameters.PageNumber,parameters.PageSize);
    }
}
