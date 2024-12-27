using ApiGestionPedidos.Api.DTOs;
using ApiGestionPedidos.Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGestionPedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly ClienteServicios _clienteServicios;
        private readonly IConfiguration _configuration;

        public AuthController(ClienteServicios clienteServicios, IConfiguration configuration)
        {
            _clienteServicios = clienteServicios;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var cliente = await _clienteServicios.AutenticarClienteAsync(dto.Email, dto.Password);

            if(cliente == null)
                return Unauthorized("Email o contraseña incorrectos.");

            //Credenciales validas, generar token
            string token = GenerarToken(cliente);
            return Ok(new { token });
        }

        private string GenerarToken(Dominio.Models.Cliente cliente)
        {
            var jwtKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim(ClaimTypes.Name, cliente.Nombre)
                //Se pueden agregar roles u otros claims 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1), //El token expira en 1 hora
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
