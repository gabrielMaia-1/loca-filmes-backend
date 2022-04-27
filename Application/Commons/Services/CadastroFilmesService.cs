using System.Linq.Expressions;
using Application.Commons.Interfaces;
using Application.Interfaces;
using Domain.Commons.Entities;

namespace Application.Commons.Services;

public class CadastroFilmesService : ICadastroFilmesService
{
    private readonly IUnitOfWork uow;

    public CadastroFilmesService(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public Filme CriaFilme(Filme filme)
    {
        throw new NotImplementedException();
    }

    public Filme DeletaFilme(int filme)
    {
        throw new NotImplementedException();
    }

    public Filme ModificaFilme(int id, Filme filme)
    {
        throw new NotImplementedException();
    }

    public List<Filme> BuscaFilme()
    {
        throw new NotImplementedException();
    }

    public List<Filme> BuscaFilme(int id)
    {
        throw new NotImplementedException();
    }
}