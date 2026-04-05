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
				.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Clave);

			if (usuario == null) return Unauthorized("Correo o contraseña incorrectos");

			return Ok(new UsuarioDTO
			{
				UsuarioId = usuario.Id,          
				Nombre = usuario.NombreCompleto, 
				Email = usuario.Email,
				Token = _jwtService.GenerarToken(usuario)
			});
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register(RegisterRequest request) 
		{
			if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
			{
				return BadRequest(new { mensaje = "El correo ya está registrado" });
			}

			var nuevoUsuario = new Usuario
			{
				NombreCompleto = request.Nombre,
				Email = request.Email,
				PasswordHash = request.Clave, 
				FechaRegistro = DateTime.UtcNow
			};

			_context.Usuarios.Add(nuevoUsuario);
			await _context.SaveChangesAsync();

			return Ok(new { mensaje = "Usuario registrado con éxito" });
		}
	}
}
