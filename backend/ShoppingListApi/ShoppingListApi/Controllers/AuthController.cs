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

        [HttpPost("solicitar-recuperacao")]
        public async Task<IActionResult> SolicitarRecuperacao([FromBody] SolicitarRecuperacaoDTO dto)
        {
            try
            {
                await _authService.SolicitarRecuperacaoSenha(dto.Email);
                return Ok(new { message = "Código de recuperação enviado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validar-codigo")]
        public async Task<IActionResult> ValidarCodigo([FromBody] ValidarCodigoDTO dto)
        {
            try
            {
                var token = await _authService.ValidarCodigoEAtualizarSenha(dto.Email, dto.Codigo, dto.NovaSenha);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}