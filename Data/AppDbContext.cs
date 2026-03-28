using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TechStoreApi.Modelo;

namespace TechStoreApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Producto> Productos { get; set; }
		public DbSet<Categoría> Categorías { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Pedido> Pedidos { get; set; }
		public DbSet<DetallePedido> DetallePedidos { get; set; }
		public DbSet<CarritoItems> CarritoItems { get; set; }
		public DbSet<Favorito> Favoritos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Producto>()
				.Property(p => p.Precio)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Producto>()
				.Property(p => p.PrecioOferta)
				.HasPrecision(18, 2);

			modelBuilder.Entity<DetallePedido>()
				.Property(d => d.PrecioUnitario)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Pedido>()
				.Property(p => p.Total)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Producto>()
				.HasOne(p => p.Categoria)
				.WithMany(c => c.Productos)
				.HasForeignKey(p => p.CategoriaId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Pedido>()
				.HasOne(p => p.Usuario)
				.WithMany()
				.HasForeignKey(p => p.UsuarioId);

			modelBuilder.Entity<Pedido>()
				.HasMany(p => p.Detalles)
				.WithOne()
				.HasForeignKey(d => d.PedidoId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
