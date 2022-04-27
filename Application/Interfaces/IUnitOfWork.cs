using Domain.Commons.Entities;

namespace Application.Interfaces;
public interface IUnitOfWork
{
    IRepository<Diretor> Diretor { get; }
    IRepository<Filme> Filme { get; }
    int Complete();
}