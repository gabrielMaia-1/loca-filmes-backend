using Domain.Commons.Entities;

namespace Test.Unit.Application.Services;

public static class TestCasesFilmes
{
    public static readonly int IdFilmeValido = 1;
    public static readonly int IdFilmeInvalido = -1;
    public static readonly  Filme FilmeValido = new Filme
    {
        Id = IdFilmeValido,
        IdDiretor = TestCasesDiretor.IdValido,
        Nome = "Filme Valido"
    };
    
    public static readonly Filme FilmeInvalidoSemDiretor = new Filme
    {
        Id = IdFilmeValido,
        Nome = "Filme Valido Sem Diretor"
    };
    
    public static readonly Filme FilmeInvalidoDiretorNaoExiste = new Filme
    {
        Id = IdFilmeValido,
        IdDiretor = TestCasesDiretor.IdInvalido,
        Nome = "Filme Valido Sem Diretor"
    };
    
    public static readonly Filme FilmeInvalidoSemNome = new Filme
    {
        Id = IdFilmeValido,
        IdDiretor = TestCasesDiretor.IdValido
    };
    
}