using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.Interfaces;
using Domain.Commons.Entities;

namespace Application.Commons.Services;

public class CadastroDiretorService : ICadastroDiretorService
{
    private IUnitOfWork uow;

    public CadastroDiretorService(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public List<Diretor> BuscaDiretor()
    {
        return uow.Diretor.GetAll().ToList();
    }

    public Diretor BuscaDiretor(int id)
    {
        var diretor = uow.Diretor.Get(id);
        return diretor ?? throw new EntidadeNaoEncontradaException(typeof(Diretor), id);
    }

    public Diretor CriaDiretor(Diretor diretor)
    {
        ArgumentNullException.ThrowIfNull(diretor);
        if(string.IsNullOrEmpty(diretor.Nome)) throw new CadastroInvalidoException("O diretor deve possuir nome.");

        var diretorRetorno = uow.Diretor.Add(diretor);
        uow.Complete();
        return diretorRetorno;
    }

    public Diretor DeletaDiretor(int id)
    {
        var diretorReferencia = uow.Diretor.Get(id);
        if(diretorReferencia is null) throw new EntidadeNaoEncontradaException(typeof(Diretor), id);
        var filmeRetorno = uow.Filme.Find(f => f.IdDiretor == diretorReferencia.Id).FirstOrDefault();
        if(filmeRetorno is not null) throw new OperacaoInvalidaException("Esta Entidade não pode ser excluida pois possui outras entidades relacionadas a ela.");
        uow.Diretor.Remove(diretorReferencia);
        uow.Complete();
        return diretorReferencia;
    }

    public Diretor ModificaDiretor(int id, Diretor diretor)
    {
        var diretorReferencia = uow.Diretor.Get(id);
        if(diretorReferencia is null) throw new EntidadeNaoEncontradaException(typeof(Diretor), id);

        diretorReferencia.Nome = diretor.Nome;

        uow.Complete();

        return diretorReferencia;
    }
}