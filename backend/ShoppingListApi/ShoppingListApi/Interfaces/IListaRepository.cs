using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;

namespace ShoppingListAPI.Interfaces
{
    public interface IListaRepository
    {
        Task<IEnumerable<ListaDTO>> GetListasByUsuario(int idUsuario);
        Task<ListaDetailDTO> GetListaById(int id);
        Task<ListaDTO> AddLista(ListaCreateDTO listaCreateDTO, int idUsuario);
        Task<ListaDTO> UpdateLista(ListaUpdateDTO listaUpdateDTO, int idLista);
        Task<bool> DeleteLista(int id);
        Task<bool> ListaExists(int id);
        Task<bool> ListaBelongsToUser(int idLista, int idUsuario);
    }
}