using Catalog_API.Models;
using Catalog_API.Pagination;

namespace Catalog_API.Repository.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoryProductsAsync();

    Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters parameters);
}
