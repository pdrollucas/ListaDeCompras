using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<string> Register(UsuarioRegisterDTO usuarioRegisterDTO)
        {
            if (await _usuarioRepository.UsuarioExists(usuarioRegisterDTO.Email))
            {
                throw new Exception("Usuário já existe");
            }

            var usuario = await _usuarioRepository.AddUsuario(usuarioRegisterDTO);
            return GenerateJwtToken(usuario);
        }

        public async Task<string> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var usuario = await _usuarioRepository.GetUsuarioByEmail(usuarioLoginDTO.Email);
            if (usuario == null || !await _usuarioRepository.CheckPassword(usuarioLoginDTO))
            {
                throw new Exception("Email ou senha inválidos");
            }

            return GenerateJwtToken(usuario);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.NomeUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}