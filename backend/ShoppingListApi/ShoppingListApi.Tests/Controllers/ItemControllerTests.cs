using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListAPI.Controllers;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models;
using ShoppingListAPI.Models.DTOs;
using System.Security.Claims;
using Xunit;

namespace ShoppingListApi.Tests.Controllers
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<IListaRepository> _mockListaRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly ItemController _controller;

        public ItemControllerTests()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockListaRepository = new Mock<IListaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _controller = new ItemController(
                _mockItemRepository.Object,
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
        public async Task GetItens_UserOwnsLista_ReturnsOkResult()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            var expectedItems = new List<Item> { new Item() };
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.GetItensByLista(listaId))
                .ReturnsAsync(expectedItems);

            // Act
            var result = await _controller.GetItens(listaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Item>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetItens_UserDoesNotOwnLista_ReturnsUnauthorized()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.GetItens(listaId);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateItem_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            int listaId = 1;
            int userId = 1;
            var itemCreateDto = new ItemCreateDTO { NomeItem = "Test Item" };
            var createdItem = new Item { IdItem = 1, NomeItem = "Test Item" };
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.AddItem(itemCreateDto, listaId))
                .ReturnsAsync(createdItem);

            // Act
            var result = await _controller.CreateItem(listaId, itemCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Item>(createdAtActionResult.Value);
            Assert.Equal("Test Item", returnValue.NomeItem);
        }

        [Fact]
        public async Task DeleteItem_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            int listaId = 1;
            int itemId = 1;
            int userId = 1;
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.ItemBelongsToLista(itemId, listaId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.DeleteItem(itemId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteItem(listaId, itemId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateItem_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            int listaId = 1;
            int itemId = 1;
            int userId = 1;
            var updateDto = new ItemUpdateDTO { NomeItem = "Updated Item" };
            var updatedItem = new Item { IdItem = itemId, NomeItem = "Updated Item" };
            
            _mockListaRepository.Setup(x => x.ListaBelongsToUser(listaId, userId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.ItemBelongsToLista(itemId, listaId))
                .ReturnsAsync(true);
            _mockItemRepository.Setup(x => x.UpdateItem(updateDto, itemId))
                .ReturnsAsync(updatedItem);

            // Act
            var result = await _controller.UpdateItem(listaId, itemId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ItemDTO>(okResult.Value);
            Assert.Equal("Updated Item", returnValue.NomeItem);
        }
    }
}
