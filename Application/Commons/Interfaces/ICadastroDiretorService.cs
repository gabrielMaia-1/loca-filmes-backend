using Domain.Commons.Entities;

namespace Application.Commons.Interfaces;

public interface ICadastroDiretorService
{
    List<Diretor> BuscaDiretor();
    Diretor BuscaDiretor(int id);
    Diretor CriaDiretor(Diretor diretor);
    Diretor DeletaDiretor(int diretor);
    Diretor ModificaDiretor(int id, Diretor diretor);
}