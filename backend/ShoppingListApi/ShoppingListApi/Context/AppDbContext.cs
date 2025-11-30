using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ShoppingListAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<Item> Itens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações de relacionamento
            modelBuilder.Entity<Lista>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Listas)
                .HasForeignKey(l => l.IdUsuario);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Lista)
                .WithMany(l => l.Itens)
                .HasForeignKey(i => i.IdLista);
        }
    }
}