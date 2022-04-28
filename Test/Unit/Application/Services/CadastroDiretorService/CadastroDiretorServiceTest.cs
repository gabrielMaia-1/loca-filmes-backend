using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Application.Commons.Exceptions;
using Application.Commons.Services;
using Application.Interfaces;
using Domain.Commons.Entities;
using Moq;
using Xunit;

namespace Test.Unit.Application.Services;

public class CadastroDiretorServiceTest
{
    private Mock<IRepository<Diretor>> mockRepository;
    private Mock<IRepository<Filme>> mockFilmeRepository;
    private Mock<IUnitOfWork> mockUow;
    private CadastroDiretorService sut;

    public CadastroDiretorServiceTest()
    {
        mockRepository = new Mock<IRepository<Diretor>>();
        mockFilmeRepository = new Mock<IRepository<Filme>>();
        mockUow = new Mock<IUnitOfWork>();

        mockUow.SetupGet(m => m.Diretor).Returns(mockRepository.Object);
        mockUow.SetupGet(m => m.Filme).Returns(mockFilmeRepository.Object);
        sut = new CadastroDiretorService(mockUow.Object);

        mockRepository.Setup(m => m.Remove(TestCasesDiretor.DiretorValido))
            .Throws<Exception>();
        mockRepository.Setup(m => m.Remove(TestCasesDiretor.DiretorDeleteValido))
            .Returns(TestCasesDiretor.DiretorDeleteValido);
        mockRepository.Setup(m => m.Get(TestCasesDiretor.IdValido))
            .Returns(TestCasesDiretor.DiretorValido);
        mockRepository.Setup(m => m.Get(TestCasesDiretor.IdDeleteValido))
            .Returns(TestCasesDiretor.DiretorDeleteValido);
        mockRepository.Setup(m => m.Add(TestCasesDiretor.DiretorValido))
            .Returns(TestCasesDiretor.DiretorValido);
    }

    #region CriaDiretor(Diretor)
        [Fact]
        public void CriaDiretor_Chama_Repository_Add_E_Complete()
        {
            sut.CriaDiretor(TestCasesDiretor.DiretorValido);

            mockRepository.Verify(m => m.Add(It.IsAny<Diretor>()), Times.Once);
            mockUow.Verify(m => m.Complete(), Times.Once);
        }

        [Fact]
        public void CriaDiretor_Retorna_Diretor_Criado()
        {
            var retornoDiretor = sut.CriaDiretor(TestCasesDiretor.DiretorValido);

            Assert.NotNull(retornoDiretor);
        }

        [Fact]
        public void CriaDiretor_Throws_ArgumentNullExcepton_Se_Diretor_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(()=> {
                sut.CriaDiretor(null!);
            });
        }

        [Fact]
        public void CriaDiretor_Throws_CadastroInvalidoException_Se_Diretor_Nao_Possuir_Nome()
        {
            Assert.Throws<CadastroInvalidoException>(()=>{
                sut.CriaDiretor(TestCasesDiretor.DiretorInvalidoSemNome);
            });
        }
    #endregion
    #region DeletaDiretor(Diretor)
        [Fact]
        public void DeletaDiretor_Chama_Repository_Remove_E_Complete()
        {
            
            sut.DeletaDiretor(TestCasesDiretor.IdDeleteValido);

            mockRepository.Verify(m => m.Remove(TestCasesDiretor.DiretorDeleteValido), Times.Once);
            mockUow.Verify(m => m.Complete(), Times.Once);
        }
        [Fact]
        public void DeletaDiretor_Retorna_Diretor_Deletado()
        {
            var retornoDiretor = sut.DeletaDiretor(TestCasesDiretor.IdDeleteValido);

            Assert.NotNull(retornoDiretor);
        }

        [Fact]
        public void DeletaDiretor_Throws_OperacaoInvalidaException_Se_Existir_Filme_Relacionado()
        {
            mockFilmeRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Filme, bool>>>()))
            .Returns(new List<Filme>() {TestCasesFilmes.FilmeValido});
            Assert.Throws<OperacaoInvalidaException>(()=> {
                sut.DeletaDiretor(TestCasesDiretor.IdValido);
            });
        }

        [Fact]
        public void DeletaDiretor_Throws_EntidadeNaoEncontradaException_Se_Diretor_For_Nulo_Ou_Nao_Existir()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(()=> {
                sut.DeletaDiretor(TestCasesDiretor.IdInvalido);
            });

            Assert.Throws<EntidadeNaoEncontradaException>(()=> {
                sut.DeletaDiretor(0);
            });
        }
    #endregion
    #region ModificaDiretor(int, Diretor)
        [Fact]
        public void ModificaDiretor_Modifica_Entidade_E_Salva()
        {
            var diretorMock = new Mock<Diretor>();

            diretorMock.SetupSet<string>(m => m.Nome = "Novo Nome")
                .Verifiable();

            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(diretorMock.Object);

            sut.ModificaDiretor(TestCasesDiretor.IdValido, new Diretor());

            diretorMock.Verify();
            mockUow.Verify(m => m.Complete());
        }

        [Fact]
        public void ModificaDiretor_Retorna_Novo_Diretor()
        {
            var diretorMock = new Mock<Diretor>();

            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(diretorMock.Object);

            var diretorRetorno = sut.ModificaDiretor(TestCasesDiretor.IdValido, new Diretor());

            Assert.NotNull(diretorRetorno);
            Assert.Equal(diretorMock.Object, diretorRetorno);
        }

        [Fact]
        public void ModificaDiretor_Throws_EntidadeNaoEncontradaException()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(()=>{
                sut.ModificaDiretor(TestCasesDiretor.IdInvalido, new Diretor());
            });
        }
    #endregion
    #region BuscaDiretor(int)
        [Fact]
        public void BuscaDiretor_Retorna_Lista_De_Diretor_Se_Existir_Diretor()
        {
            var filmes = new List<Diretor>()
            {
                new Diretor(),
                new Diretor()
            };

            mockRepository.Setup(m => m.GetAll())
                .Returns(filmes);

            var filmesRetorno = sut.BuscaDiretor();

            //Assert
            Assert.NotEmpty(filmes);
        }

        [Fact]
        public void BuscaDiretor_Retorna_Lista_Vazia_Se_Nao_Existir_Diretor()
        {
            var filmes = new List<Diretor>();

            mockRepository.Setup(m => m.GetAll())
                .Returns(filmes);

            var filmesRetorno = sut.BuscaDiretor();

            //Assert
            Assert.Empty(filmes);
        }

        [Fact]
        public void BuscaDiretor_Por_Id_Retorna_Diretor_Se_Existir()
        {
            var filmeRetorno = sut.BuscaDiretor(TestCasesDiretor.IdValido);

            //Assert
            Assert.NotNull(filmeRetorno);
        }

        [Fact]
        public void BuscaDiretor_Por_Id_Throws_EntidadeNaoEncontradaException_Se_Diretor_Nao_Existir()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(() => { 
                sut.BuscaDiretor(TestCasesDiretor.IdInvalido);
            });
        }
    #endregion

}