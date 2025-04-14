using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models.DTOs;
using System.Security.Claims;

namespace ShoppingListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListaController : ControllerBase
    {
        private readonly IListaRepository _listaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ListaController(IListaRepository listaRepository, IUsuarioRepository usuarioRepository)
        {
            _listaRepository = listaRepository;
            _usuarioRepository = usuarioRepository;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetListas()
        {
            var listas = await _listaRepository.GetListasByUsuario(GetUserId());
            return Ok(listas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLista(int id)
        {
            if (!await _listaRepository.ListaBelongsToUser(id, GetUserId()))
                return Unauthorized();

            var lista = await _listaRepository.GetListaById(id);
            if (lista == null) return NotFound();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLista(ListaCreateDTO listaCreateDTO)
        {
            var lista = await _listaRepository.AddLista(listaCreateDTO, GetUserId());
            return CreatedAtAction(nameof(GetLista), new { id = lista.IdLista }, lista);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLista(int id, ListaUpdateDTO listaUpdateDTO)
        {
            if (!await _listaRepository.ListaBelongsToUser(id, GetUserId()))
                return Unauthorized();

            var lista = await _listaRepository.UpdateLista(listaUpdateDTO, id);
            if (lista == null) return NotFound();

            return Ok(lista);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLista(int id)
        {
            if (!await _listaRepository.ListaBelongsToUser(id, GetUserId()))
                return Unauthorized();

            var result = await _listaRepository.DeleteLista(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}