using Catalog_API.Context;
using Catalog_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog_API.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected CatalogApiContext _context;

    public Repository(CatalogApiContext context)
    {
        _context = context;
    }

    //IQueryable is a filter that will be passed to the database to filter and return the results. If you use IEnumerable, the filter in memory will be applied after fetching all records
    public IQueryable<T> Get()
    {
        //Set<T> of _context returns a DbSet<T> instance to access entities of the received generic T type
        return _context.Set<T>().AsNoTracking();
        /* When adding the AsNoTracking() method (include reference to Microsoft.EntityFrameworkCore),
         * the EntityFramework stops monitoring the state of objects, that is, the query becomes lighter.
         * However, _context loses update information on these objects, so this optimization can only be
         * done in methods that will not update this data in the database (perfect for GET verbs)*/
    }

    public async Task<T> GetByIdAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        //Set<T> of _context returns a DbSet<T> instance to access entities of the received generic T type
        return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public void Add(T entity)
    {
        //Set<T> of _context returns a DbSet<T> instance to access entities of the received generic T type
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        //Set<T> of _context returns a DbSet<T> instance to access entities of the received generic T type
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        //Set<T> of _context returns a DbSet<T> instance to access entities of the received generic T type
        _context.Set<T>().Update(entity);
    }
}
