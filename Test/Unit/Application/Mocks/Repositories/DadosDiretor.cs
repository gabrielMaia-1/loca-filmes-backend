using System;
using System.Collections.Generic;
using Application.Interfaces;
using Domain.Commons.Entities;
using Moq;

namespace Test.Unit.Application.Services;

public class DadosDiretor
{
    public static int IdNaoDeletavel = 1;
    public static Diretor NaoDeletavel = new Diretor
    {
        Id = IdNaoDeletavel,
        Nome = "Diretor Nao Deletavel."
    };

    public static int IdDeletavel = 2;
    public static Diretor Deletavel = new Diretor
    {
        Id = IdDeletavel,
        Nome = "Diretor Deletavel."
    };
    public static Diretor CadastroValido = new Diretor
    {
        Nome = "Registro Cadastravel."
    };
    public static Diretor CadastroInvalidoSemNome = new Diretor();
    public static int IdNaoExiste = 99;
    public static int IdExiste = IdDeletavel;
    public static Diretor Existe = NaoDeletavel;

    public static List<Filme> FilmesRelacionados = new List<Filme>
    {
        new Filme()
        {
            Id = 1,
            IdDiretor = IdNaoDeletavel,
            Nome = "Filme Relacionado com Diretor Nao Deletavel"
        }
    };

    public static List<Diretor> ListaDiretores = new List<Diretor>
    {
        Deletavel, NaoDeletavel
    };
    
    public Mock<IRepository<Diretor>> MockDiretorRepository { get; }
    public Mock<IRepository<Filme>> MockFilmeRepository { get; }
    public Mock<IUnitOfWork> MockUow { get; }

    public DadosDiretor()
    {
        MockDiretorRepository = new Mock<IRepository<Diretor>>();
        MockFilmeRepository = new Mock<IRepository<Filme>>();
        MockUow = new Mock<IUnitOfWork>();
        MockUow.SetupGet(m => m.Filme).Returns(MockFilmeRepository.Object);
        MockUow.SetupGet(m => m.Diretor).Returns(MockDiretorRepository.Object);


        //Registro Existe e Causa Erro de Fk
        MockDiretorRepository.Setup(m => m.Get(IdNaoDeletavel))
            .Returns(NaoDeletavel);

        MockDiretorRepository.Setup(m => m.Remove(NaoDeletavel))
            .Throws<Exception>();

        //Registro Existe e Nao Causa Erro de Fk
        MockDiretorRepository.Setup(m => m.Get(IdDeletavel))
            .Returns(Deletavel);

        MockDiretorRepository.Setup(m => m.Remove(Deletavel))
            .Returns(Deletavel);
        
        //Registro Nao Existe na base
        MockDiretorRepository.Setup(m => m.Get(IdNaoExiste))
            .Returns(() => null);

        //Registro Valido P/ Cadastro
        MockDiretorRepository.Setup(m => m.Add(CadastroValido))
            .Returns(CadastroValido);

        //Registro Valido P/ Cadastro
        MockDiretorRepository.Setup(m => m.Add(CadastroInvalidoSemNome))
            .Throws<Exception>();
    }
}