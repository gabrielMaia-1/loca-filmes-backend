namespace Application.Commons.Exceptions;

public class EntidadeNaoEncontradaException : Exception
{
    public EntidadeNaoEncontradaException()
    {
    }
    public EntidadeNaoEncontradaException(string message) : base(message)
    {
    }

    public EntidadeNaoEncontradaException(Type tipo, int id)
        : this($"A Entidade do tipo {tipo.Name}, de Id = {id} não existe ou nao foi encontrada.")
    {
    }
}