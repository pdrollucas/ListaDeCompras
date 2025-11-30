namespace ShoppingListAPI.Models.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class UsuarioLoginDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class UsuarioRegisterDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string NomeUsuario { get; set; }
    }
}