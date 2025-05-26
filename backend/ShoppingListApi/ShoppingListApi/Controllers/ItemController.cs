using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models.DTOs;
using System.Security.Claims;

namespace ShoppingListAPI.Controllers
{
    [Authorize]
    [Route("api/lista/{listaId}/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IListaRepository _listaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ItemController(
            IItemRepository itemRepository, 
            IListaRepository listaRepository,
            IUsuarioRepository usuarioRepository)
        {
            _itemRepository = itemRepository;
            _listaRepository = listaRepository;
            _usuarioRepository = usuarioRepository;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetItens(int listaId)
        {
            if (!await _listaRepository.ListaBelongsToUser(listaId, GetUserId()))
                return Unauthorized();

            var itens = await _itemRepository.GetItensByLista(listaId);
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int listaId, int id)
        {
            if (!await _listaRepository.ListaBelongsToUser(listaId, GetUserId()))
                return Unauthorized();

            var item = await _itemRepository.GetItemById(id);
            if (item == null || !await _itemRepository.ItemBelongsToLista(id, listaId))
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(int listaId, ItemCreateDTO itemCreateDTO)
        {
            if (!await _listaRepository.ListaBelongsToUser(listaId, GetUserId()))
                return Unauthorized();

            var item = await _itemRepository.AddItem(itemCreateDTO, listaId);
            return CreatedAtAction(nameof(GetItem), new { listaId, id = item.IdItem }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int listaId, int id, ItemUpdateDTO itemUpdateDTO)
        {
            if (!await _listaRepository.ListaBelongsToUser(listaId, GetUserId()))
                return Unauthorized();

            if (!await _itemRepository.ItemBelongsToLista(id, listaId))
                return NotFound();

            var item = await _itemRepository.UpdateItem(itemUpdateDTO, id);
            if (item == null) return NotFound();

            var itemDto = new ItemDTO
            {
                IdItem = item.IdItem,
                NomeItem = item.NomeItem,
                Quantidade = item.Quantidade
            };

            return Ok(itemDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int listaId, int id)
        {
            if (!await _listaRepository.ListaBelongsToUser(listaId, GetUserId()))
                return Unauthorized();

            if (!await _itemRepository.ItemBelongsToLista(id, listaId))
                return NotFound();

            var result = await _itemRepository.DeleteItem(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}