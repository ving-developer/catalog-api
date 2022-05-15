using System.Linq.Expressions;

namespace Catalog_API.Repository.Interfaces;

public interface IRepository<T>
{
    IQueryable<T> Get();
    T GetById(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
