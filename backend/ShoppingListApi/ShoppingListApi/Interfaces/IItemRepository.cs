using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;

namespace ShoppingListAPI.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItensByLista(int idLista);
        Task<Item> GetItemById(int id);
        Task<Item> AddItem(ItemCreateDTO itemCreateDTO, int idLista);
        Task<Item> UpdateItem(ItemUpdateDTO itemUpdateDTO, int idItem);
        Task<bool> DeleteItem(int id);
        Task<bool> ItemExists(int id);
        Task<bool> ItemBelongsToLista(int idItem, int idLista);
    }
}