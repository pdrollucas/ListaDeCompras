using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;

namespace ShoppingListAPI.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetUsuarioById(int id);
        Task<Usuario> GetUsuarioByEmail(string email);
        Task<Usuario> AddUsuario(UsuarioRegisterDTO usuarioRegisterDTO);
        Task<bool> UsuarioExists(string email);
        Task<bool> CheckPassword(UsuarioLoginDTO usuarioLoginDTO);
        Task UpdatePassword(int userId, string newPassword);
    }
}