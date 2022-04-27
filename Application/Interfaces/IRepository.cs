
using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? Get(int id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    TEntity Add(TEntity entity);
    TEntity Remove(TEntity entity);
}