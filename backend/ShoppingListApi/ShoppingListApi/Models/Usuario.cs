using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Senha { get; set; }

        [Required, MaxLength(50)]
        public string NomeUsuario { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Lista> Listas { get; set; }
    }
}