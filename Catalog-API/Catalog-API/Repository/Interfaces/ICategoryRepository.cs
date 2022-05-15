using Catalog_API.Models;

namespace Catalog_API.Repository.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoryProducts();
}
