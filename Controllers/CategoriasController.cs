using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;

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

		[HttpPost]
		public async Task<ActionResult<Categoría>> PostCategoria(Categoría categoria)
		{
			_context.Categorías.Add(categoria);
			await _context.SaveChangesAsync();
			return Ok(categoria);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCategoria(int id, Categoría categoria)
		{
			if (id != categoria.Id) return BadRequest();
			_context.Entry(categoria).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategoria(int id)
		{
			var c = await _context.Categorías.FindAsync(id);
			if (c == null) return NotFound();
			_context.Categorías.Remove(c);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Categoría eliminada" });
		}
	}
}
