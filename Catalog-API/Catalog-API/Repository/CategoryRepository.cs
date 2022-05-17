using Catalog_API.Context;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog_API.Repository;

public class CategoryRepository: Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CatalogApiContext context) : base(context) { }

    public async Task<IEnumerable<Category>> GetCategoryProductsAsync()
    {
        /*The "Include" extension method allows you to load related entities,
         * in this example it will load the product entities related to the
         * categories. The "Take(2)" method used in Products, specifies a
         * limit of products related to the categories to be searched.*/
        return await Get().Include(x => x.Products.Take(2)).ToListAsync();
    }

    public PagedList<Category> GetCategories(CategoryParameters parameters)
    {
        return PagedList<Category>.ToPagedList(Get().OrderBy(c => c.CategoryId),
            parameters.PageNumber, parameters.PageSize);
    }
}
