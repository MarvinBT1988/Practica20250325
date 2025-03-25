using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practica20250325.AppMVCImagenData.Models;

public partial class Practica20250325IdataDbContext : DbContext
{
    public Practica20250325IdataDbContext()
    {
    }

    public Practica20250325IdataDbContext(DbContextOptions<Practica20250325IdataDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE836D10317C");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
