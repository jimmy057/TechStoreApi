using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public ProductosController(AppDbContext context) => _context = context;

		// GET: api/productos (Opcional filtrar por categoria)
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductos(int? categoriaId = null)
		{
			var query = _context.Productos.Include(p => p.Categoria).AsQueryable();

			if (categoriaId.HasValue)
				query = query.Where(p => p.CategoriaId == categoriaId);

			return await query.Select(p => new ProductoDTO
			{
				Id = p.Id,
				Nombre = p.Nombre,
				Marca = p.Marca,
				Precio = p.Precio,
				PrecioOferta = p.PrecioOferta,
				ImagenUrl = p.ImagenUrl,
				NombreCategoria = p.Categoria != null ? p.Categoria.Nombre : "General",
				RAM = p.RAM,
				Procesador = p.Procesador,
				Calificacion = p.Calificacion
			}).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
		{
			var p = await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(x => x.Id == id);
			if (p == null) return NotFound();

			return new ProductoDTO
			{
				Id = p.Id,
				Nombre = p.Nombre,
				Precio = p.Precio,
				ImagenUrl = p.ImagenUrl,
				NombreCategoria = p.Categoria?.Nombre ?? "",
				RAM = p.RAM,
				Procesador = p.Procesador
			};
		}
	}
}
