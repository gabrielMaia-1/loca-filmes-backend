using System.Linq.Expressions;
using Application.Interfaces;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly PgContext Context;

    public Repository(PgContext context)
    {
        Context = context;
    }

    public TEntity? Get(int id)
    {
        return Context.Set<TEntity>().Find(id);
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return Context.Set<TEntity>().ToList();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate);
    }

    public TEntity Add(TEntity entity)
    {
        return Context.Set<TEntity>().Add(entity).Entity;
    }

    public TEntity Remove(TEntity entity)
    {
        return Context.Set<TEntity>().Remove(entity).Entity;
    }
}