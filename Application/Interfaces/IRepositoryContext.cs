using Domain.Commons.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;
    
    public interface IRepositoryContext
    {
        DbSet<Diretor> Cidade { get; }
        DbSet<Filme> Endereco { get; }
    }