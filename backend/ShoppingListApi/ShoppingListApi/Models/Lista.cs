using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class Lista
    {
        [Key]
        public int IdLista { get; set; }

        [Required, MaxLength(100)]
        public string NomeLista { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Item> Itens { get; set; }
    }
}