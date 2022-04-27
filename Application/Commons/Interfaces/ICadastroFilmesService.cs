using Domain.Commons.Entities;

namespace Application.Commons.Interfaces;

public interface ICadastroFilmesService
{
    List<Filme> BuscaFilme();
    List<Filme> BuscaFilme(int id);
    Filme CriaFilme(Filme filme);
    Filme DeletaFilme(int filme);
    Filme ModificaFilme(int id, Filme filme);
}