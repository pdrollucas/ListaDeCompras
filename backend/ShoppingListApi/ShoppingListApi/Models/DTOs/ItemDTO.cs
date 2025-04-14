namespace ShoppingListAPI.Models.DTOs
{
    public class ItemDTO
    {
        public int IdItem { get; set; }
        public string NomeItem { get; set; }
        public int Quantidade { get; set; }
        public int IdLista { get; set; }
    }

    public class ItemCreateDTO
    {
        public string NomeItem { get; set; }
        public int Quantidade { get; set; } = 1;
    }

    public class ItemUpdateDTO
    {
        public string NomeItem { get; set; }
        public int Quantidade { get; set; }
    }
}