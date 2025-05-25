using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShoppingListAPI.Models;
using System.Net.Mail;
using System.Net;

namespace ShoppingListAPI.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private static Dictionary<string, (string codigo, DateTime expiracao)> _codigosRecuperacao = new();

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

        public async Task<bool> SolicitarRecuperacaoSenha(string email)
        {
            var usuario = await _usuarioRepository.GetUsuarioByEmail(email);
            if (usuario == null)
            {
                throw new Exception("Email não encontrado");
            }

            var codigo = GerarCodigoAleatorio();
            _codigosRecuperacao[email] = (codigo, DateTime.Now.AddMinutes(15));

            await EnviarEmailRecuperacao(email, codigo);
            return true;
        }

        public async Task<string> ValidarCodigoEAtualizarSenha(string email, string codigo, string novaSenha)
        {
            if (!_codigosRecuperacao.ContainsKey(email))
            {
                throw new Exception("Nenhum código de recuperação solicitado para este email");
            }

            var (codigoSalvo, expiracao) = _codigosRecuperacao[email];
            if (DateTime.Now > expiracao)
            {
                _codigosRecuperacao.Remove(email);
                throw new Exception("Código expirado");
            }

            if (codigo != codigoSalvo)
            {
                throw new Exception("Código inválido");
            }

            var usuario = await _usuarioRepository.GetUsuarioByEmail(email);
            await _usuarioRepository.UpdatePassword(usuario.IdUsuario, novaSenha);

            _codigosRecuperacao.Remove(email);
            return GenerateJwtToken(usuario);
        }

        private string GerarCodigoAleatorio()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task EnviarEmailRecuperacao(string email, string codigo)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            using var client = new SmtpClient()
            {
                Host = smtpServer,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(username),
                Subject = "Recuperação de Senha - Lista de Compras",
                Body = $"Seu código de recuperação de senha é: {codigo}\n\nEste código expira em 15 minutos.",
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}