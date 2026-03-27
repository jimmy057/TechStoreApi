using Microsoft.AspNetCore.Mvc;
using TechStoreApi.Data;
using TechStoreApi.Modelo;
using TechStoreApi.DTOs;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PedidosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public PedidosController(AppDbContext context) => _context = context;

		[HttpPost]
		public async Task<ActionResult> PostPedido(Pedido pedido)
		{
			_context.Pedidos.Add(pedido);
			await _context.SaveChangesAsync();
			return Ok(new { pedidoId = pedido.Id, mensaje = "Compra realizada" });
		}
	}
}
