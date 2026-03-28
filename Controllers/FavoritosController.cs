using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.Modelo;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FavoritosController : ControllerBase
	{
		private readonly AppDbContext _context;
		public FavoritosController(AppDbContext context) => _context = context;

		[HttpPost]
		public async Task<ActionResult> Add(Favorito fav)
		{
			_context.Favoritos.Add(fav);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("{usuarioId}")]
		public async Task<ActionResult> Get(int usuarioId) =>
			Ok(await _context.Favoritos.Where(f => f.UsuarioId == usuarioId).Include(f => f.Producto).ToListAsync());

		[HttpDelete("{usuarioId}/{productoId}")]
		public async Task<IActionResult> Delete(int usuarioId, int productoId)
		{
			var fav = await _context.Favoritos
				.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.ProductoId == productoId);

			if (fav == null) return NotFound(new { mensaje = "El producto no está en favoritos" });

			_context.Favoritos.Remove(fav);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Eliminado de favoritos" });
		}
	}
}
