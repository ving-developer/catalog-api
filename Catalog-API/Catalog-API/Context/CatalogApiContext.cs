using Catalog_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog_API.Context
{
    /*
     * Auxiliary class for the relationship between the Models entities and the application database
     * It must contain all the objects of the Models in the DbSet, for the EntityFremework to relate/create tables
     */
    public class CatalogApiContext : DbContext
    {
        public CatalogApiContext(DbContextOptions<CatalogApiContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
