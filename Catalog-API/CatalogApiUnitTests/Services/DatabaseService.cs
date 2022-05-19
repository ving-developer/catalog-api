using Catalog_API.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatalogApiUnitTests.Services;

[CollectionDefinition("DatabaseServiceCollection")]
public class DatabaseService : ICollectionFixture<DatabaseService>
{
    private static readonly DbContextOptions<CatalogApiContext> _options = new DbContextOptionsBuilder<CatalogApiContext>()
                                                    .UseInMemoryDatabase("CatalogApiTestDb")
                                                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                    .Options;
    private static readonly CatalogApiContext _context;
    public readonly CatalogApiContext Context;

    static DatabaseService()//the static constructor will be called only once, before the first instance of this class is created
    {
        //Create context instance
        _context = new CatalogApiContext(_options);
        _context.Database.EnsureCreated();
        //Adding data
        #region Adding Categories
        _context.Categories.Add(Constants.CategoryOne);
        _context.Categories.Add(Constants.CategoryTwo);
        _context.Categories.Add(Constants.CategoryThree);
        _context.Categories.Add(Constants.CategoryFour);
        #endregion

        #region Adding Products
        _context.Products.Add(Constants.ProductOne);
        _context.Products.Add(Constants.ProductTwo);
        _context.Products.Add(Constants.ProductThree);
        _context.Products.Add(Constants.ProductFour);
        _context.Products.Add(Constants.ProductFive);
        _context.Products.Add(Constants.ProductSix);
        #endregion

        _context.SaveChanges();
    }

    public DatabaseService()
    {
        Context = new CatalogApiContext(_options);
    }
}
