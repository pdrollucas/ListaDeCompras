using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListAPI.Controllers;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models.DTOs;
using System.Security.Claims;
using Xunit;

namespace ShoppingListApi.Tests.Controllers
{
    public class ListaControllerTests
    {
        private readonly Mock<IListaRepository> _mockListaRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly ListaController _controller;

        public ListaControllerTests()
        {
            _mockListaRepository = new Mock<IListaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _controller = new ListaController(
                _mockListaRepository.Object,
                _mockUsuarioRepository.Object
            );

            // Setup ClaimsPrincipal
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetListas_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var expectedListas = new List<ListaDTO> { new ListaDTO() };
            
            _mockListaRepository.Setup(x => x.GetListasByUsuario(userId))
                .ReturnsAsync(expectedListas);

            // Act
            var result = await _controller.GetListas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ListaDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetLista_UserOwnsList_ReturnsOkResult()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            var expectedLista = new ListaDetailDTO { IdLista = listaId, NomeLista = "Test Lista" };
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockListaRepository.Setup(x => x.GetListaById(listaId))
                .ReturnsAsync(expectedLista);

            // Act
            var result = await _controller.GetLista(listaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ListaDetailDTO>(okResult.Value);
            Assert.Equal(listaId, returnValue.IdLista);
        }

        [Fact]
        public async Task GetLista_UserDoesNotOwnList_ReturnsUnauthorized()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.GetLista(listaId);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateLista_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            int userId = 1;
            var listaCreateDto = new ListaCreateDTO { NomeLista = "New Lista" };
            var createdLista = new ListaDTO { IdLista = 1, NomeLista = "New Lista" };
            
            _mockListaRepository.Setup(x => x.AddLista(listaCreateDto, userId))
                .ReturnsAsync(createdLista);

            // Act
            var result = await _controller.CreateLista(listaCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ListaDTO>(createdAtActionResult.Value);
            Assert.Equal("New Lista", returnValue.NomeLista);
        }

        [Fact]
        public async Task UpdateLista_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            var updateDto = new ListaUpdateDTO { NomeLista = "Updated Lista" };
            var updatedLista = new ListaDTO { IdLista = listaId, NomeLista = "Updated Lista" };
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockListaRepository.Setup(x => x.UpdateLista(updateDto, listaId))
                .ReturnsAsync(updatedLista);

            // Act
            var result = await _controller.UpdateLista(listaId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ListaDTO>(okResult.Value);
            Assert.Equal("Updated Lista", returnValue.NomeLista);
        }

        [Fact]
        public async Task DeleteLista_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockListaRepository.Setup(x => x.DeleteLista(listaId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteLista(listaId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
