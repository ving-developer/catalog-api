using Catalog_API.Context;
using Catalog_API.Models;
using Catalog_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog_API.Repository;

public class CategoryRepository: Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CatalogApiContext context) : base(context) { }

    public IEnumerable<Category> GetCategoryProducts()
    {
        /*The "Include" extension method allows you to load related entities,
         * in this example it will load the product entities related to the
         * categories. The "Take(2)" method used in Products, specifies a
         * limit of products related to the categories to be searched.*/
        return Get().Include(x => x.Products.Take(2));
    }
}
