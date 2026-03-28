using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarritoController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CarritoController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet("{usuarioId}")]
		public async Task<ActionResult<IEnumerable<CarritoItems>>> GetCarrito(int usuarioId)
		{
			var items = await _context.CarritoItems
				.Where(c => c.UsuarioId == usuarioId)
				.Include(c => c.Producto) 
				.ToListAsync();

			return Ok(items);
		}

		[HttpPost("agregar")]
		public async Task<ActionResult> AgregarAlCarrito(CarritoItemDto dto)
		{
			var itemExistente = await _context.CarritoItems
				.FirstOrDefaultAsync(c => c.UsuarioId == dto.UsuarioId && c.ProductoId == dto.ProductoId);

			if (itemExistente != null)
			{
				itemExistente.Cantidad += dto.Cantidad;
				_context.CarritoItems.Update(itemExistente);
			}
			else
			{
				var nuevoItem = new CarritoItems
				{
					UsuarioId = dto.UsuarioId,
					ProductoId = dto.ProductoId,
					Cantidad = dto.Cantidad
				};
				_context.CarritoItems.Add(nuevoItem);
			}

			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Producto actualizado en el carrito" });
		}

		[HttpDelete("eliminar/{id}")]
		public async Task<IActionResult> EliminarItem(int id)
		{
			var item = await _context.CarritoItems.FindAsync(id);
			if (item == null) return NotFound();

			_context.CarritoItems.Remove(item);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Producto eliminado del carrito" });
		}

		[HttpDelete("vaciar/{usuarioId}")]
		public async Task<IActionResult> VaciarCarrito(int usuarioId)
		{
			var items = await _context.CarritoItems.Where(c => c.UsuarioId == usuarioId).ToListAsync();
			_context.CarritoItems.RemoveRange(items);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Carrito vaciado" });
		}
	}
}