using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApi.Data;
using TechStoreApi.DTOs;
using TechStoreApi.Modelo;
using TechStoreApi.Services; 

namespace TechStoreApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly JwtService _jwtService; 

		public AuthController(AppDbContext context, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
		}

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
				Token = _jwtService.GenerarToken(usuario) 
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
