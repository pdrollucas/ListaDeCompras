using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Context;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;

namespace ShoppingListAPI.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetItensByLista(int idLista)
        {
            return await _context.Itens
                .Where(i => i.IdLista == idLista)
                .ToListAsync();
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Itens.FindAsync(id);
        }

        public async Task<Item> AddItem(ItemCreateDTO itemCreateDTO, int idLista)
        {
            var item = new Item
            {
                NomeItem = itemCreateDTO.NomeItem,
                Quantidade = itemCreateDTO.Quantidade,
                IdLista = idLista
            };

            _context.Itens.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateItem(ItemUpdateDTO itemUpdateDTO, int idItem)
        {
            var item = await GetItemById(idItem);
            if (item == null) return null;

            item.NomeItem = itemUpdateDTO.NomeItem;
            item.Quantidade = itemUpdateDTO.Quantidade;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await GetItemById(id);
            if (item == null) return false;

            _context.Itens.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ItemExists(int id)
        {
            return await _context.Itens.AnyAsync(i => i.IdItem == id);
        }

        public async Task<bool> ItemBelongsToLista(int idItem, int idLista)
        {
            return await _context.Itens.AnyAsync(i => i.IdItem == idItem && i.IdLista == idLista);
        }
    }
}