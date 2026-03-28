using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PedidosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public PedidosController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<ActionResult> PostPedido(PedidoDto pedidoDto)
		{
			if (pedidoDto.Detalles == null || !pedidoDto.Detalles.Any())
			{
				return BadRequest(new { mensaje = "El carrito está vacío." });
			}

			var nuevoPedido = new Pedido
			{
				UsuarioId = pedidoDto.UsuarioId,
				Total = pedidoDto.Total,
				DireccionEnvio = pedidoDto.DireccionEnvio,
				FechaPedido = DateTime.UtcNow,
				Estado = "Pendiente",
				Detalles = pedidoDto.Detalles.Select(d => new DetallePedido
				{
					ProductoId = d.ProductoId,
					Cantidad = d.Cantidad,
					PrecioUnitario = d.PrecioUnitario
				}).ToList()
			};

			try
			{
				_context.Pedidos.Add(nuevoPedido);
				await _context.SaveChangesAsync();

				return Ok(new
				{
					pedidoId = nuevoPedido.Id,
					mensaje = "¡Compra procesada con éxito!",
					cliente = pedidoDto.UsuarioId,
					destino = nuevoPedido.DireccionEnvio
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					mensaje = "Error al guardar el pedido",
					detalles = ex.InnerException?.Message ?? ex.Message
				});
			}
		}

		[HttpGet("usuario/{usuarioId}")]
		public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorUsuario(int usuarioId)
		{
			var pedidos = await _context.Pedidos
				.Where(p => p.UsuarioId == usuarioId)
				.Include(p => p.Detalles)
				.OrderByDescending(p => p.FechaPedido)
				.ToListAsync();

			return Ok(pedidos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Pedido>> GetPedido(int id)
		{
			var pedido = await _context.Pedidos
				.Include(p => p.Detalles)
				.ThenInclude(d => d.Producto)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (pedido == null) return NotFound();

			return Ok(pedido);
		}
	}
}
