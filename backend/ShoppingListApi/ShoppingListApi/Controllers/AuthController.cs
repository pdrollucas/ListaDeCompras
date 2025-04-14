using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models.DTOs;
using ShoppingListAPI.Services;

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UsuarioRegisterDTO usuarioRegisterDTO)
        {
            try
            {
                var token = await _authService.Register(usuarioRegisterDTO);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            try
            {
                var token = await _authService.Login(usuarioLoginDTO);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}