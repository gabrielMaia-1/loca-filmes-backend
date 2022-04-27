using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain.Commons.Entities;

namespace Infra.Contexts
{
    public partial class PgContext : DbContext
    {
        public PgContext()
        {
        }

        public PgContext(DbContextOptions<PgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diretor> Diretor { get; set; } = null!;
        public virtual DbSet<Filme> Filme { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diretor>(entity =>
            {
                entity.ToTable("diretor", "cad");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('cad.diretor_seq'::regclass)");

                entity.Property(e => e.Nome)
                    .HasColumnType("character varying")
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Filme>(entity =>
            {
                entity.ToTable("filme", "cad");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('cad.filme_seq'::regclass)");

                entity.Property(e => e.IdDiretor).HasColumnName("id_diretor");

                entity.Property(e => e.Nome)
                    .HasColumnType("character varying")
                    .HasColumnName("nome");

                entity.HasOne(d => d.IdDiretorNavigation)
                    .WithMany(p => p.Filme)
                    .HasForeignKey(d => d.IdDiretor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("filme_diretor_pk");
            });

            modelBuilder.HasSequence("diretor_seq", "cad");

            modelBuilder.HasSequence("filme_seq", "cad");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
