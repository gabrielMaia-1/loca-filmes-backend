using System;
using System.Collections.Generic;
using Application.Interfaces;
using Domain.Commons.Entities;
using Moq;

namespace Test.Unit.Application.Services;

public class DadosFilme
{
    public static readonly int IdDiretor = 1;
    public static readonly int IdDiretorNaoExiste = 99;
    public static readonly Diretor Diretor = new Diretor
    {
        Id = IdDiretor,
        Nome = "Diretor"
    };

    public static readonly int IdExiste = 1;
    public static readonly Filme Existe = new Filme
    {
        Id = IdExiste,
        IdDiretor = IdDiretor,
        Nome = "Filme Existe",
    };

    public static readonly List<Filme> ListaFilmes = new List<Filme>
    {
        Existe
    };
    
    public static readonly int IdNaoExiste = 99;

    public static readonly  Filme CadastroValido = new Filme
    {
        IdDiretor = DadosFilme.IdDiretor,
        Nome = "Filme Valido"
    };
    
    public static readonly Filme CadastroDiretorNulo = new Filme
    {
        Nome = "Filme Valido Sem Diretor"
    };
    
    public static readonly Filme CadastroDiretorNaoExiste = new Filme
    {
        IdDiretor = DadosFilme.IdDiretorNaoExiste,
        Nome = "Filme Valido Sem Diretor"
    };
    
    public static readonly Filme CadastroSemNome = new Filme
    {
        IdDiretor = DadosFilme.IdDiretor
    };

    public Mock<IUnitOfWork> MockUoW { get; }
    public Mock<IRepository<Filme>> MockFilmeRepository { get; }
    public Mock<IRepository<Diretor>> MockDiretorRepository { get; }

    public DadosFilme()
    {
        MockFilmeRepository = new Mock<IRepository<Filme>>();
        MockDiretorRepository = new Mock<IRepository<Diretor>>();
        MockUoW = new Mock<IUnitOfWork>();
        MockUoW.SetupGet(m => m.Filme).Returns(MockFilmeRepository.Object);
        MockUoW.SetupGet(m => m.Diretor).Returns(MockDiretorRepository.Object);

        //Filme Existe no repositorio
        MockFilmeRepository.Setup(m => m.Get(IdExiste))
            .Returns(Existe);

        //Filme Nao Existe no repositorio
        MockFilmeRepository.Setup(m => m.Get(IdNaoExiste))
            .Returns(() => null);

        //Lista de Filems no repositorio
        MockFilmeRepository.Setup(m => m.GetAll())
            .Returns(ListaFilmes);

        //Filme Sem nome
        MockFilmeRepository.Setup(m => m.Add(It.Is<Filme>(f => string.IsNullOrWhiteSpace(f.Nome))))
            .Throws<Exception>();

        //Cadastro Valido
        MockFilmeRepository.Setup(m => m.Add(It.Is<Filme>(f => f == CadastroValido)))
            .Returns(CadastroValido);
        
        //Filme Sem Diretor
        MockFilmeRepository.Setup(m => m.Add(It.Is<Filme>(f => f.IdDiretor == 0)))
            .Throws<Exception>();

        //Diretor Nao Existe no repositorio
        MockDiretorRepository.Setup(m => m.Get(It.Is<int>(i => i != IdDiretor)))
            .Returns(() => null);

        //Diretor Existe no repositorio
        MockDiretorRepository.Setup(m => m.Get(It.Is<int>(i => i == IdDiretor)))
            .Returns(Diretor);
    }

}