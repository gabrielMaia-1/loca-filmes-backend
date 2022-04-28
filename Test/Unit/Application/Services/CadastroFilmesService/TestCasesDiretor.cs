using Domain.Commons.Entities;

namespace Test.Unit.Application.Services;

public static class TestCasesDiretor
{
    public static int IdValido = 1;
    public static int IdDeleteValido = 2;
    public static int IdInvalido = -1;
    public static Diretor DiretorValido = new Diretor
    {
        Id = IdValido,
        Nome = "Nome Diretor Valido"
    };

    public static Diretor DiretorDeleteValido = new Diretor
    {
        Id = IdDeleteValido,
        Nome = "Nome Diretor Delete Valido"
    };

    public static Diretor DiretorInvalidoSemNome = new Diretor
    {
        Id = IdValido
    };
}