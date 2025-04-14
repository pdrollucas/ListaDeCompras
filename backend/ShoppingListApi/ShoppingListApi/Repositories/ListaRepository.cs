using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Context;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;

namespace ShoppingListAPI.Repositories
{
    public class ListaRepository : IListaRepository
    {
        private readonly AppDbContext _context;

        public ListaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListaDTO>> GetListasByUsuario(int idUsuario)
        {
            return await _context.Listas
                .Where(l => l.IdUsuario == idUsuario)
                .Select(l => new ListaDTO
                {
                    IdLista = l.IdLista,
                    NomeLista = l.NomeLista,
                    IdUsuario = l.IdUsuario
                })
                .ToListAsync();
        }

        public async Task<ListaDetailDTO> GetListaById(int id)
        {
            return await _context.Listas
                .Include(l => l.Itens)
                .Where(l => l.IdLista == id)
                .Select(l => new ListaDetailDTO
                {
                    IdLista = l.IdLista,
                    NomeLista = l.NomeLista,
                    IdUsuario = l.IdUsuario,
                    Itens = l.Itens.Select(i => new ItemDTO
                    {
                        IdItem = i.IdItem,
                        NomeItem = i.NomeItem,
                        Quantidade = i.Quantidade,
                        IdLista = i.IdLista
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ListaDTO> AddLista(ListaCreateDTO listaCreateDTO, int idUsuario)
        {
            var lista = new Lista
            {
                NomeLista = listaCreateDTO.NomeLista,
                IdUsuario = idUsuario
            };

            _context.Listas.Add(lista);
            await _context.SaveChangesAsync();

            return new ListaDTO
            {
                IdLista = lista.IdLista,
                NomeLista = lista.NomeLista,
                IdUsuario = lista.IdUsuario
            };
        }

        public async Task<ListaDTO> UpdateLista(ListaUpdateDTO listaUpdateDTO, int idLista)
        {
            var lista = await _context.Listas.FindAsync(idLista);
            if (lista == null) return null;

            lista.NomeLista = listaUpdateDTO.NomeLista;
            await _context.SaveChangesAsync();

            return new ListaDTO
            {
                IdLista = lista.IdLista,
                NomeLista = lista.NomeLista,
                IdUsuario = lista.IdUsuario
            };
        }

        public async Task<bool> DeleteLista(int id)
        {
            var lista = await _context.Listas.FindAsync(id);
            if (lista == null) return false;

            _context.Listas.Remove(lista);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ListaExists(int id)
        {
            return await _context.Listas.AnyAsync(l => l.IdLista == id);
        }

        public async Task<bool> ListaBelongsToUser(int idLista, int idUsuario)
        {
            return await _context.Listas.AnyAsync(l => l.IdLista == idLista && l.IdUsuario == idUsuario);
        }
    }
}