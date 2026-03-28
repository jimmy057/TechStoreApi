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
	}
}
