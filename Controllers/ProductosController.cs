using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;
using System;

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
			var query = _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Imagenes)
				.AsQueryable();

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
				Calificacion = p.Calificacion,
				Galeria = p.Imagenes.Select(i => i.Url).ToList()
			}).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
		{
			var p = await _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Imagenes)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (p == null) return NotFound();

			return new ProductoDTO
			{
				Id = p.Id,
				Nombre = p.Nombre,
				Precio = p.Precio,
				ImagenUrl = p.ImagenUrl,
				NombreCategoria = p.Categoria?.Nombre ?? "",
				RAM = p.RAM,
				Procesador = p.Procesador,
				Galeria = p.Imagenes.Select(i => i.Url).ToList()
			};
		}

		[HttpGet("buscar")]
		public async Task<ActionResult<IEnumerable<ProductoDTO>>> BuscarProductos([FromQuery] string q)
		{
			return await _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Imagenes)
				.Where(p => p.Nombre.ToLower().Contains(q.ToLower()) || p.Marca.ToLower().Contains(q.ToLower()))
				.Select(p => new ProductoDTO
				{
					Id = p.Id,
					Nombre = p.Nombre,
					Marca = p.Marca,
					Precio = p.Precio,
					ImagenUrl = p.ImagenUrl,
					NombreCategoria = p.Categoria != null ? p.Categoria.Nombre : "General",
					Galeria = p.Imagenes.Select(i => i.Url).ToList()
				}).ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<Producto>> PostProducto(Producto producto)
		{
			foreach (var property in producto.GetType().GetProperties())
			{
				if (property.PropertyType == typeof(DateTime))
				{
					var currentValue = (DateTime)property.GetValue(producto);
					property.SetValue(producto, DateTime.SpecifyKind(currentValue, DateTimeKind.Utc));
				}
				else if (property.PropertyType == typeof(DateTime?))
				{
					var currentValue = (DateTime?)property.GetValue(producto);
					if (currentValue.HasValue)
					{
						property.SetValue(producto, DateTime.SpecifyKind(currentValue.Value, DateTimeKind.Utc));
					}
				}
			}

			_context.Productos.Add(producto);
			await _context.SaveChangesAsync();
			return Ok(producto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutProducto(int id, Producto producto)
		{
			if (id != producto.Id) return BadRequest(new { mensaje = "El ID no coincide" });

			foreach (var property in producto.GetType().GetProperties())
			{
				if (property.PropertyType == typeof(DateTime))
				{
					var currentValue = (DateTime)property.GetValue(producto);
					property.SetValue(producto, DateTime.SpecifyKind(currentValue, DateTimeKind.Utc));
				}
				else if (property.PropertyType == typeof(DateTime?))
				{
					var currentValue = (DateTime?)property.GetValue(producto);
					if (currentValue.HasValue)
					{
						property.SetValue(producto, DateTime.SpecifyKind(currentValue.Value, DateTimeKind.Utc));
					}
				}
			}

			_context.Entry(producto).State = EntityState.Modified;

			try { await _context.SaveChangesAsync(); }
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.Productos.Any(e => e.Id == id)) return NotFound();
				else throw;
			}
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProducto(int id)
		{
			var p = await _context.Productos.FindAsync(id);
			if (p == null) return NotFound();
			_context.Productos.Remove(p);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Producto eliminado" });
		}

		[HttpPost("{productoId}/imagenes")]
		public async Task<ActionResult> AgregarImagenGaleria(int productoId, [FromBody] string urlImagen)
		{
			var producto = await _context.Productos.FindAsync(productoId);
			if (producto == null) return NotFound(new { mensaje = "Producto no encontrado" });

			var nuevaImagen = new ImagenProducto
			{
				ProductoId = productoId,
				Url = urlImagen
			};

			_context.ImagenesProducto.Add(nuevaImagen);
			await _context.SaveChangesAsync();

			return Ok(new { mensaje = "Imagen agregada a la galería con éxito" });
		}
	}
}
