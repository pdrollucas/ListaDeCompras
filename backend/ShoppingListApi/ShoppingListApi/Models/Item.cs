using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class Item
    {
        [Key]
        public int IdItem { get; set; }

        [Required, MaxLength(100)]
        public string NomeItem { get; set; }

        public int Quantidade { get; set; } = 1;

        public int IdLista { get; set; }
        public Lista Lista { get; set; }
    }
}