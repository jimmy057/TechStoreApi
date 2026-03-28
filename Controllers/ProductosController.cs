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

		// NUEVO: Buscador para la App Android
		[HttpGet("buscar")]
		public async Task<ActionResult<IEnumerable<ProductoDTO>>> BuscarProductos([FromQuery] string q)
		{
			return await _context.Productos
				.Include(p => p.Categoria)
				.Where(p => p.Nombre.ToLower().Contains(q.ToLower()) || p.Marca.ToLower().Contains(q.ToLower()))
				.Select(p => new ProductoDTO
				{
					Id = p.Id,
					Nombre = p.Nombre,
					Marca = p.Marca,
					Precio = p.Precio,
					ImagenUrl = p.ImagenUrl,
					NombreCategoria = p.Categoria != null ? p.Categoria.Nombre : "General"
				}).ToListAsync();
		}

		// NUEVO (ADMIN): Crear producto
		[HttpPost]
		public async Task<ActionResult<Producto>> PostProducto(Producto producto)
		{
			_context.Productos.Add(producto);
			await _context.SaveChangesAsync();
			return Ok(producto);
		}

		// NUEVO (ADMIN): Editar producto (Para actualizar stock manualmente, por ejemplo)
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProducto(int id, Producto producto)
		{
			if (id != producto.Id) return BadRequest(new { mensaje = "El ID no coincide" });

			_context.Entry(producto).State = EntityState.Modified;

			try { await _context.SaveChangesAsync(); }
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.Productos.Any(e => e.Id == id)) return NotFound();
				else throw;
			}
			return NoContent();
		}

		// NUEVO (ADMIN): Borrar producto
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProducto(int id)
		{
			var p = await _context.Productos.FindAsync(id);
			if (p == null) return NotFound();
			_context.Productos.Remove(p);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Producto eliminado" });
		}
	}
}
