using Catalog_API.Models;
using Catalog_API.Pagination;

namespace Catalog_API.Repository.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoryProducts();

    PagedList<Category> GetCategories(CategoryParameters parameters);
}
