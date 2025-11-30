using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Context;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingListAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> AddUsuario(UsuarioRegisterDTO usuarioRegisterDTO)
        {
            var usuario = new Usuario
            {
                Email = usuarioRegisterDTO.Email,
                NomeUsuario = usuarioRegisterDTO.NomeUsuario,
                Senha = HashPassword(usuarioRegisterDTO.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> UsuarioExists(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckPassword(UsuarioLoginDTO usuarioLoginDTO)
        {
            var usuario = await GetUsuarioByEmail(usuarioLoginDTO.Email);
            if (usuario == null) return false;

            return VerifyPassword(usuarioLoginDTO.Senha, usuario.Senha);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashOfInput = HashPassword(inputPassword);
            return string.Equals(hashOfInput, storedHash);
        }

        public async Task UpdatePassword(int userId, string newPassword)
        {
            var usuario = await GetUsuarioById(userId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            usuario.Senha = HashPassword(newPassword);
            await _context.SaveChangesAsync();
        }
    }
}