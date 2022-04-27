using Application.Interfaces;
using Domain.Commons.Entities;
using Infra.Contexts;
using Infra.Repository;

namespace Infra.UnitOfWork;

class UnitOfWork : IUnitOfWork
{
    private readonly PgContext _context;
    public IRepository<Filme> Filme { get; }
    public IRepository<Diretor> Diretor { get; }

    public UnitOfWork(PgContext context)
    {
        _context = context;
        Filme = new Repository<Filme>(_context);
        Diretor = new Repository<Diretor>(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }
}