using System;
using System.Collections.Generic;
using ES2_Gestao_Eventos.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ES2_Gestao_Eventos.Database.Context;

public partial class GestaoEventosDBContext : DbContext
{
    public GestaoEventosDBContext()
    {
    }

    public GestaoEventosDBContext(DbContextOptions<GestaoEventosDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bilhete> Bilhetes { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Tipobilhete> Tipobilhetes { get; set; }

    public virtual DbSet<Tipouser> Tipousers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(" Host=localhost;Database=GestaoEventos;Username=postgres;Password=sofiaam");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bilhete>(entity =>
        {
            entity.HasKey(e => e.IdBilhete).HasName("bilhetes_pk");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Bilhetes).HasConstraintName("eventos_idevento_fk");

            entity.HasOne(d => d.IdTipoBilhetesNavigation).WithMany(p => p.Bilhetes).HasConstraintName("tipobilhetes_idtipobilhetes_fk");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("categorias_pk");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("evento_pk");

            entity.HasOne(d => d.IdCategoriaNavigation)
                .WithMany(p => p.Eventos)
                .HasConstraintName("categorias_idcategoria_fk");

            entity.HasOne(d => d.IdUserNavigation)
                .WithMany(p => p.Eventos)
                .HasConstraintName("user_iduser_fk");
        });

        modelBuilder.Entity<Tipobilhete>(entity =>
        {
            entity.HasKey(e => e.IdTipoBilhete).HasName("tipobilhete_pk");
        });

        modelBuilder.Entity<Tipouser>(entity =>
        {
            entity.HasKey(e => e.IdTipoUser).HasName("tipousers_pk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pk");

            entity.HasOne(d => d.TipoUserNavigation).WithMany(p => p.Users).HasConstraintName("tipouser_idtipouser_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
