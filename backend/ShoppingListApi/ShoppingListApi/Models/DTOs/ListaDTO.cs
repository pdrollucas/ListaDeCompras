using System.Collections.Generic;

namespace ShoppingListAPI.Models.DTOs
{
    public class ListaDTO
    {
        public int IdLista { get; set; }
        public string NomeLista { get; set; }
        public int IdUsuario { get; set; }
    }

    public class ListaDetailDTO : ListaDTO
    {
        public ICollection<ItemDTO> Itens { get; set; } = new List<ItemDTO>();
    }

    public class ListaCreateDTO
    {
        public string NomeLista { get; set; }
    }

    public class ListaUpdateDTO
    {
        public string NomeLista { get; set; }
    }
}