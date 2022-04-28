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
    private Mock<IRepository<Diretor>> MockDiretorRepository;
    private Mock<IRepository<Filme>> MockFilmeRepository;
    private Mock<IUnitOfWork> MockUow;
    private CadastroDiretorService Sut;

    public CadastroDiretorServiceTest()
    {
        var dadosTesteDiretor = new DadosDiretor();
        MockDiretorRepository = dadosTesteDiretor.MockDiretorRepository;
        MockFilmeRepository = dadosTesteDiretor.MockFilmeRepository;
        MockUow = dadosTesteDiretor.MockUow;
        Sut = new CadastroDiretorService(MockUow.Object);
    }

    #region CriaDiretor(Diretor)
        [Fact]
        public void CriaDiretor_Chama_Repository_Add_E_Complete()
        {
            Sut.CriaDiretor(DadosDiretor.CadastroValido);

            MockDiretorRepository.Verify(m => m.Add(It.IsAny<Diretor>()), Times.Once);
            MockUow.Verify(m => m.Complete(), Times.Once);
        }

        [Fact]
        public void CriaDiretor_Retorna_Diretor_Criado()
        {
            var retornoDiretor = Sut.CriaDiretor(DadosDiretor.CadastroValido);

            Assert.NotNull(retornoDiretor);
        }

        [Fact]
        public void CriaDiretor_Throws_ArgumentNullExcepton_Se_Diretor_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(()=> {
                Sut.CriaDiretor(null!);
            });
        }

        [Fact]
        public void CriaDiretor_Throws_CadastroInvalidoException_Se_Diretor_Nao_Possuir_Nome()
        {
            Assert.Throws<CadastroInvalidoException>(()=>{
                Sut.CriaDiretor(DadosDiretor.CadastroInvalidoSemNome);
            });
        }
    #endregion
    #region DeletaDiretor(Diretor)
        [Fact]
        public void DeletaDiretor_Chama_Repository_Remove_E_Complete()
        {
            Sut.DeletaDiretor(DadosDiretor.IdDeletavel);

            MockDiretorRepository.Verify(m => m.Remove(DadosDiretor.Deletavel), Times.Once);
            MockUow.Verify(m => m.Complete(), Times.Once);
        }
        [Fact]
        public void DeletaDiretor_Retorna_Diretor_Deletado()
        {
            var retornoDiretor = Sut.DeletaDiretor(DadosDiretor.IdDeletavel);

            Assert.NotNull(retornoDiretor);
        }

        [Fact]
        public void DeletaDiretor_Throws_OperacaoInvalidaException_Se_Existir_Filme_Relacionado()
        {
            MockFilmeRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Filme, bool>>>()))
            .Returns(DadosDiretor.FilmesRelacionados);
            Assert.Throws<OperacaoInvalidaException>(()=> {
                Sut.DeletaDiretor(DadosDiretor.IdNaoDeletavel);
            });
        }

        [Fact]
        public void DeletaDiretor_Throws_EntidadeNaoEncontradaException_Se_Diretor_For_Nulo_Ou_Nao_Existir()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(()=> {
                Sut.DeletaDiretor(DadosDiretor.IdNaoExiste);
            });

            Assert.Throws<EntidadeNaoEncontradaException>(()=> {
                Sut.DeletaDiretor(0);
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

            MockDiretorRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(diretorMock.Object);

            Sut.ModificaDiretor(DadosDiretor.IdExiste, new Diretor());

            diretorMock.Verify();
            MockUow.Verify(m => m.Complete());
        }

        [Fact]
        public void ModificaDiretor_Retorna_Novo_Diretor()
        {
            var diretorMock = new Mock<Diretor>();

            MockDiretorRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(diretorMock.Object);

            var diretorRetorno = Sut.ModificaDiretor(DadosDiretor.IdExiste, new Diretor());

            Assert.NotNull(diretorRetorno);
            Assert.Equal(diretorMock.Object, diretorRetorno);
        }

        [Fact]
        public void ModificaDiretor_Throws_EntidadeNaoEncontradaException()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(()=>{
                Sut.ModificaDiretor(DadosDiretor.IdNaoExiste, new Diretor());
            });
        }
    #endregion
    #region BuscaDiretor(int)
        [Fact]
        public void BuscaDiretor_Retorna_Lista_De_Diretor_Se_Existir_Diretor()
        {
            MockDiretorRepository.Setup(m => m.GetAll())
                .Returns(DadosDiretor.ListaDiretores);

            var filmesRetorno = Sut.BuscaDiretor();

            //Assert
            Assert.NotEmpty(filmesRetorno);
        }

        [Fact]
        public void BuscaDiretor_Retorna_Lista_Vazia_Se_Nao_Existir_Diretor()
        {
            MockDiretorRepository.Setup(m => m.GetAll())
                .Returns(new List<Diretor>());

            var filmesRetorno = Sut.BuscaDiretor();

            //Assert
            Assert.Empty(filmesRetorno);
        }

        [Fact]
        public void BuscaDiretor_Por_Id_Retorna_Diretor_Se_Existir()
        {
            var filmeRetorno = Sut.BuscaDiretor(DadosDiretor.IdExiste);

            //Assert
            Assert.NotNull(filmeRetorno);
        }

        [Fact]
        public void BuscaDiretor_Por_Id_Throws_EntidadeNaoEncontradaException_Se_Diretor_Nao_Existir()
        {
            Assert.Throws<EntidadeNaoEncontradaException>(() => { 
                Sut.BuscaDiretor(DadosDiretor.IdNaoExiste);
            });
        }
    #endregion

}