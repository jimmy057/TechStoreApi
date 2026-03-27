using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;

		public AuthController(AppDbContext context) => _context = context;

		[HttpPost("login")]
		public async Task<ActionResult<UsuarioDTO>> Login(LoginRequest request)
		{
			var usuario = await _context.Usuarios
				.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password); 

			if (usuario == null) return Unauthorized("Correo o contraseña incorrectos");

			return new UsuarioDTO
			{
				Id = usuario.Id,
				NombreCompleto = usuario.NombreCompleto,
				Email = usuario.Email,
				Token = "fake-jwt-token-para-pruebas"
			};
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register(Usuario usuario)
		{
			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();
			return Ok(new { mensaje = "Usuario registrado con éxito" });
		}
	}
}
