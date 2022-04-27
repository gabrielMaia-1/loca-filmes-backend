using System.Linq.Expressions;
using Application.Commons.Exceptions;
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
        ArgumentNullException.ThrowIfNull(filme);
        if(string.IsNullOrWhiteSpace(filme.Nome)) throw new CadastroInvalidoException("Nome não pode ser vazio");
        if(filme.IdDiretor == 0) throw new CadastroInvalidoException("Filme deve possuir um diretor.");

        var diretor = uow.Diretor.Get(filme.IdDiretor);
        if(diretor is null) throw new EntidadeNaoEncontradaException(typeof(Diretor), filme.IdDiretor);

        var filmeRetorno = uow.Filme.Add(filme);
        uow.Complete();
        return filmeRetorno;
    }

    public Filme DeletaFilme(int id)
    {
       ArgumentNullException.ThrowIfNull(id);

       var filme =  uow.Filme.Get(id);

       if(filme is null) throw new EntidadeNaoEncontradaException(typeof(Filme), id);
       uow.Filme.Remove(filme);
       uow.Complete();
       return filme;
    }

    public Filme ModificaFilme(int id, Filme filme)
    {
        ArgumentNullException.ThrowIfNull(filme);
        ArgumentNullException.ThrowIfNull(id);

       var filmeAntigo =  uow.Filme.Get(id);

       if(filmeAntigo is null) throw new EntidadeNaoEncontradaException(typeof(Filme), id);



       filmeAntigo.Nome = filme.Nome;
       filmeAntigo.IdDiretor = filme.IdDiretor;

       return filme;
    }

    public List<Filme> BuscaFilme()
    {
        return uow.Filme.GetAll().ToList();
    }

    public Filme BuscaFilme(int id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var filme = uow.Filme.Get(id);
        if(filme is null) throw new EntidadeNaoEncontradaException(typeof(Filme), id);

        return filme;
    }
}