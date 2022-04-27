using System;
namespace Application.Commons.Exceptions;
public class CadastroInvalidoException : Exception
{
    public CadastroInvalidoException()
    {
    }
    public CadastroInvalidoException(string message) : base(message)
    {
    }
}