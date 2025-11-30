namespace ShoppingListAPI.Models.DTOs
{
    public class SolicitarRecuperacaoDTO
    {
        public string Email { get; set; }
    }

    public class ValidarCodigoDTO
    {
        public string Email { get; set; }
        public string Codigo { get; set; }
        public string NovaSenha { get; set; }
    }
}
