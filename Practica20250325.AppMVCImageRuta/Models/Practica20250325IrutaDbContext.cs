using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practica20250325.AppMVCImageRuta.Models;

public partial class Practica20250325IrutaDbContext : DbContext
{
    public Practica20250325IrutaDbContext()
    {
    }

    public Practica20250325IrutaDbContext(DbContextOptions<Practica20250325IrutaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE83B4133B2C");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.ImagenRuta)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
