using Domain.Commons.Entities;

namespace Test.Unit.Application.Services;

public static class TestCasesDiretor
{
    public static int IdValido = 1;
    public static int IdInvalido = -1;
    public static Diretor DiretorValido = new Diretor
    {
        Id = IdValido,
        Nome = "Nome Diretor"
    };
}