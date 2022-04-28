using System;
namespace Application.Commons.Exceptions;
public class OperacaoInvalidaException : Exception
{
    public OperacaoInvalidaException()
    {
    }
    public OperacaoInvalidaException(string message) : base(message)
    {
    }
}