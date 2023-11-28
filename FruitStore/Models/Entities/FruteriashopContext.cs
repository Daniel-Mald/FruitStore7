using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FruitStore.Models.Entities;

public partial class FruteriashopContext : DbContext
{
    public FruteriashopContext()
    {
    }

    public FruteriashopContext(DbContextOptions<FruteriashopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Productos> Productos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("categorias")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.Nombre, "NombreGrupo");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("productos")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.IdCategoria, "GruposProductos");

            entity.HasIndex(e => e.Id, "IdProducto");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasPrecision(19, 4);
            entity.Property(e => e.UnidadMedida).HasMaxLength(45);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("fk_categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
