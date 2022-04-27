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
    private readonly Mock<IRepository<Filme>> mockFilmeRepository;
    private readonly Mock<IRepository<Diretor>> mockDiretorRepository;
    private readonly Mock<IUnitOfWork> mockUow;
    private readonly CadastroFilmesService sut;

    public CadastroFilmesServiceTest()
    {
        mockFilmeRepository = new Mock<IRepository<Filme>>();
        mockDiretorRepository = new Mock<IRepository<Diretor>>();

        mockUow = new Mock<IUnitOfWork>();
        mockUow.SetupGet(m => m.Filme).Returns(mockFilmeRepository.Object);
        mockUow.SetupGet(m => m.Diretor).Returns(mockDiretorRepository.Object);
        sut = new CadastroFilmesService(mockUow.Object);
 
        mockFilmeRepository.Setup(m => m.Add(TestCasesFilmes.FilmeValido))
            .Returns(TestCasesFilmes.FilmeValido);
        mockFilmeRepository.Setup(m => m.Add(TestCasesFilmes.FilmeInvalidoSemDiretor))
            .Throws<Exception>();
        mockFilmeRepository.Setup(m => m.Add(TestCasesFilmes.FilmeInvalidoSemNome))
            .Throws<Exception>();
        mockFilmeRepository.Setup(m => m.Add(TestCasesFilmes.FilmeInvalidoDiretorNaoExiste))
            .Throws<Exception>();
        mockFilmeRepository.Setup(m => m.Remove(TestCasesFilmes.FilmeValido))
            .Returns(TestCasesFilmes.FilmeValido);
        mockFilmeRepository.Setup(m => m.Get(TestCasesFilmes.IdFilmeValido))
            .Returns(TestCasesFilmes.FilmeValido);

        mockDiretorRepository.Setup(m => m.Get(TestCasesDiretor.IdValido))
            .Returns(TestCasesDiretor.DiretorValido);
        
    }

    #region CriaFilme
    [Fact]
    public void CriaFilme_Chama_Repository_Add_E_Complete()
    {
        // When
        sut.CriaFilme(TestCasesFilmes.FilmeValido);

        // Then
        mockFilmeRepository.Verify(m => m.Add(It.IsAny<Filme>()), Times.Once);
        mockUow.Verify(m => m.Complete(), Times.Once);
    }

    [Fact]
    public void CriaFilme_Retorna_Filme_Criado()
    {
        // When
        var retornoFilme = sut.CriaFilme(TestCasesFilmes.FilmeValido);

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
            sut.CriaFilme(TestCasesFilmes.FilmeInvalidoSemDiretor);
        });

        Assert.Throws<CadastroInvalidoException>(()=>{
            sut.CriaFilme(TestCasesFilmes.FilmeInvalidoSemNome);
        });
    }

    [Fact]
    public void CriaFilme_Throws_EntidadeNaoEncontradaException_Se_Diretor_Informado_Nao_Existir()
    {
        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=>{
            sut.CriaFilme(TestCasesFilmes.FilmeInvalidoDiretorNaoExiste);
        });
    }
    #endregion
    #region DeletaFilme
    [Fact]
    public void DeletaFilme_Chama_Repository_Remove_E_Complete()
    {
        // When
        sut.DeletaFilme(TestCasesFilmes.IdFilmeValido);

        // Then
        mockFilmeRepository.Verify(m => m.Remove(It.IsAny<Filme>()), Times.Once);
        mockUow.Verify(m => m.Complete(), Times.Once);
    }
    [Fact]
    public void DeletaFilme_Retorna_Filme_Criado()
    {
        // When
        var retornoFilme = sut.DeletaFilme(TestCasesFilmes.IdFilmeValido);

        // Then
        Assert.NotNull(retornoFilme);
    }

    [Fact]
    public void DeletaFilme_Throws_EntidadeNaoEncontradaException_Se_Filme_For_Nulo_Ou_Nao_Existir()
    {
        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=> {
            sut.DeletaFilme(TestCasesFilmes.IdFilmeInvalido);
        });
    }
    #endregion
    #region BuscaFilme
    [Fact]
    public void BuscaFilme_Retorna_Lista_De_Filme_Se_Existir_Filmes()
    {
        // Given
        var filmes = new List<Filme>()
        {
            new Filme(),
            new Filme()
        };

        mockFilmeRepository.Setup(m => m.GetAll())
            .Returns(filmes);

        // When
        var filmesRetorno = sut.BuscaFilme();

        //Assert
        Assert.NotEmpty(filmes);
    }

    [Fact]
    public void BuscaFilme_Retorna_Lista_Vazia_Se_Nao_Existir_Filmes()
    {
        // Given
        var filmes = new List<Filme>();

        mockFilmeRepository.Setup(m => m.GetAll())
            .Returns(filmes);

        // When
        var filmesRetorno = sut.BuscaFilme();

        //Assert
        Assert.Empty(filmes);
    }

    [Fact]
    public void BuscaFilme_Por_Id_Retorna_Filme_Se_Existir()
    {
        // When
        var filmeRetorno = sut.BuscaFilme(TestCasesFilmes.IdFilmeValido);

        //Assert
        Assert.NotNull(filmeRetorno);
    }

    [Fact]
    public void BuscaFilme_Por_Id_Throws_EntidadeNaoEncontradaException_Se_Filme_Nao_Existir()
    {
        Assert.Throws<EntidadeNaoEncontradaException>(() => { 
            sut.BuscaFilme(TestCasesFilmes.IdFilmeInvalido);
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
            Id = TestCasesFilmes.IdFilmeValido,
            IdDiretor = TestCasesDiretor.IdValido,
            Nome = "Novo Nome"
        };

        // When
        var retornoFilme = sut.ModificaFilme(TestCasesFilmes.IdFilmeValido, filmeNovo);

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
            Id = TestCasesFilmes.IdFilmeValido,
            IdDiretor = TestCasesDiretor.IdValido,
            Nome = "Novo Nome"
        };

        // When
        Assert.Throws<EntidadeNaoEncontradaException>(()=>{
            sut.ModificaFilme(TestCasesFilmes.IdFilmeInvalido, filmeNovo);
        });
    }
    #endregion

}