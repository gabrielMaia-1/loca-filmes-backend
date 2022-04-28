using System.Collections.Generic;
using System;
using Application.Interfaces;
using Application.Commons.Services;
using Domain.Commons.Entities;
using Moq;
using Xunit;
using Application.Commons.Exceptions;

namespace Test.Unit.Application.Services;

public class CadastroFilmesServiceTest
{
    private readonly Mock<IRepository<Filme>> MockFilmeRepository;
    private readonly Mock<IRepository<Diretor>> MockDiretorRepository;
    private readonly Mock<IUnitOfWork> MockUow;
    private readonly CadastroFilmesService sut;

    public CadastroFilmesServiceTest()
    {

        var dadosFilme = new DadosFilme();
        
        MockFilmeRepository = dadosFilme.MockFilmeRepository;
        MockDiretorRepository = dadosFilme.MockDiretorRepository;
        MockUow = dadosFilme.MockUoW;

        sut = new CadastroFilmesService(MockUow.Object);        
    }

    #region CriaFilme
    [Fact]
    public void CriaFilme_Chama_Repository_Add_E_Complete()
    {
        // When
        sut.CriaFilme(DadosFilme.CadastroValido);

        // Then
        MockFilmeRepository.Verify(m => m.Add(It.IsAny<Filme>()), Times.Once);
        MockUow.Verify(m => m.Complete(), Times.Once);
    }

    [Fact]
    public void CriaFilme_Retorna_Filme_Criado()
    {
        // When
        var retornoFilme = sut.CriaFilme(DadosFilme.CadastroValido);

        // Then
        Assert.NotNull(retornoFilme);
    }

    [Fact]
    public void CriaFilme_Throws_ArgumentNullExcepton_Se_Filme_For_Nulo()
    {
        // When
        Assert.Throws<ArgumentNullException>(()=> {
            sut.CriaFilme(null!);
        });
    }
    
    [Fact]
    public void CriaFilme_Throws_OperacaoInvalidaException_Se_Filme_Nao_Possuir_Diretor_Ou_Nome()
    {
        // When
        Assert.Throws<CadastroInvalidoException>(()=>{
            sut.CriaFilme(DadosFilme.CadastroDiretorNulo);
        });

        Assert.Throws<CadastroInvalidoException>(()=>{
            sut.CriaFilme(DadosFilme.CadastroSemNome);
        });
    }

    [Fact]
    public void CriaFilme_Throws_EntidadeNaoEncontradaException_Se_Diretor_Informado_Nao_Existir()
    {
        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=>{
            sut.CriaFilme(DadosFilme.CadastroDiretorNaoExiste);
        });
    }
    #endregion
    #region DeletaFilme
    [Fact]
    public void DeletaFilme_Chama_Repository_Remove_E_Complete()
    {
        // When
        sut.DeletaFilme(DadosFilme.IdExiste);

        // Then
        MockFilmeRepository.Verify(m => m.Remove(It.IsAny<Filme>()), Times.Once);
        MockUow.Verify(m => m.Complete(), Times.Once);
    }
    [Fact]
    public void DeletaFilme_Retorna_Filme_Deletado()
    {
        // When
        var retornoFilme = sut.DeletaFilme(DadosFilme.IdExiste);

        // Then
        Assert.NotNull(retornoFilme);
    }

    [Fact]
    public void DeletaFilme_Throws_EntidadeNaoEncontradaException_Se_Filme_For_Nulo_Ou_Nao_Existir()
    {
        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=> {
            sut.DeletaFilme(DadosFilme.IdNaoExiste);
        });
    }
    #endregion
    #region BuscaFilme
    [Fact]
    public void BuscaFilme_Retorna_Lista_De_Filme_Se_Existir_Filmes()
    {
        MockFilmeRepository.Setup(m => m.GetAll())
            .Returns(DadosFilme.ListaFilmes);

        // When
        var filmesRetorno = sut.BuscaFilme();

        //Assert
        Assert.NotEmpty(filmesRetorno);
    }

    [Fact]
    public void BuscaFilme_Retorna_Lista_Vazia_Se_Nao_Existir_Filmes()
    {
        // Given
        var filmes = new List<Filme>();

        MockFilmeRepository.Setup(m => m.GetAll())
            .Returns(filmes);

        // When
        var filmesRetorno = sut.BuscaFilme();

        //Assert
        Assert.Empty(filmesRetorno);
    }

    [Fact]
    public void BuscaFilme_Por_Id_Retorna_Filme_Se_Existir()
    {
        // When
        var filmeRetorno = sut.BuscaFilme(DadosFilme.IdExiste);

        //Assert
        Assert.NotNull(filmeRetorno);
    }

    [Fact]
    public void BuscaFilme_Por_Id_Throws_EntidadeNaoEncontradaException_Se_Filme_Nao_Existir()
    {
        Assert.Throws<EntidadeNaoEncontradaException>(() => { 
            sut.BuscaFilme(DadosFilme.IdNaoExiste);
        });
    }
    #endregion
    #region ModificaFilme
    [Fact]
    public void ModificaFilme_Retorna_Filme_Modificado()
    {
        // Given
        var filmeNovo = new Filme
        {
            IdDiretor = DadosFilme.IdDiretor,
            Nome = "Novo Nome"
        };

        // When
        var retornoFilme = sut.ModificaFilme(DadosFilme.IdExiste, filmeNovo);

        // Then
        Assert.NotNull(retornoFilme);
        Assert.Equal(filmeNovo.Nome, retornoFilme.Nome);
        Assert.Equal(filmeNovo.IdDiretor, retornoFilme.IdDiretor);
    }

    [Fact]
    public void ModificaFilme_Throws_EntidadeNaoEncontradaException_Se_Filme_Nao_Existe()
    {
        // Given
        var filmeNovo = new Filme
        {
            IdDiretor = DadosFilme.IdDiretor,
            Nome = "Novo Nome"
        };

        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=>{
            sut.ModificaFilme(DadosFilme.IdNaoExiste, filmeNovo);
        });
    }
    #endregion

}