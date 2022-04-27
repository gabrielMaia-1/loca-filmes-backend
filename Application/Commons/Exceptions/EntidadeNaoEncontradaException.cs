namespace Application.Commons.Exceptions;

public class EntidadeNaoEncontradaException : Exception
{
    public EntidadeNaoEncontradaException()
    {
    }
    public EntidadeNaoEncontradaException(string message) : base(message)
    {
    }
}