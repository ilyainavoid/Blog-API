using System.Linq.Expressions;

namespace BlogApi.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<T?> Get(Guid id);
    Task<List<T>> GetAll();
    IEnumerable<T> GetSpecified(Expression<Func<T, bool>> expression);
    Task Insert(T entity);
    Task Delete(T entity);
    Task Update(T entity);
    Task InsertCollection(List<T> entities);
    Task DeleteCollection(List<T> entities);
}