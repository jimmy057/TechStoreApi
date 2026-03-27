using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriasController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CategoriasController(AppDbContext context) => _context = context;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
		{
			return await _context.Categorías
				.Select(c => new CategoriaDTO
				{
					Id = c.Id,
					Nombre = c.Nombre,
					ImagenIconoUrl = c.ImagenIconoUrl
				}).ToListAsync();
		}
	}
}
