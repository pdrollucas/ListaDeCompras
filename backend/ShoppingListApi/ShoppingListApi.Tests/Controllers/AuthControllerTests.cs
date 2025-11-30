using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListAPI.Controllers;
using ShoppingListAPI.Models.DTOs;
using ShoppingListAPI.Services;
using Microsoft.Extensions.Configuration;
using Xunit;
using ShoppingListAPI.Interfaces;
using ShoppingListAPI.Models;
using System.Dynamic;

namespace ShoppingListApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigSection;
        private readonly AuthService _authService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfigSection = new Mock<IConfigurationSection>();
            
            _mockConfigSection.Setup(x => x.Value).Returns("chave-secreta-para-testes-1234567890");
            _mockConfiguration.Setup(x => x.GetSection("Jwt:Key")).Returns(_mockConfigSection.Object);
            
            _authService = new AuthService(_mockUsuarioRepository.Object, _mockConfiguration.Object);
            _controller = new AuthController(_authService);
        }

        [Fact]
        public async Task Register_Success_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new UsuarioRegisterDTO
            {
                Email = "test@test.com",
                Senha = "123456",
                NomeUsuario = "Test User"
            };

            _mockUsuarioRepository.Setup(x => x.UsuarioExists(registerDto.Email))
                .ReturnsAsync(false);

            _mockUsuarioRepository.Setup(x => x.AddUsuario(registerDto))
                .ReturnsAsync(new Usuario
                {
                    IdUsuario = 1,
                    Email = registerDto.Email,
                    NomeUsuario = registerDto.NomeUsuario
                });

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okResult.Value.GetType().GetProperty("token") != null, "Response should contain a 'token' property");
            Assert.NotNull(okResult.Value.GetType().GetProperty("token").GetValue(okResult.Value, null));
        }

        [Fact]
        public async Task Register_Failure_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new UsuarioRegisterDTO
            {
                Email = "test@test.com",
                Senha = "123456",
                NomeUsuario = "Test User"
            };

            _mockUsuarioRepository.Setup(x => x.UsuarioExists(registerDto.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário já existe", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_Success_ReturnsOkResult()
        {
            // Arrange
            var loginDto = new UsuarioLoginDTO
            {
                Email = "test@test.com",
                Senha = "123456"
            };

            var usuario = new Usuario
            {
                IdUsuario = 1,
                Email = loginDto.Email,
                NomeUsuario = "Test User"
            };

            _mockUsuarioRepository.Setup(x => x.GetUsuarioByEmail(loginDto.Email))
                .ReturnsAsync(usuario);

            _mockUsuarioRepository.Setup(x => x.CheckPassword(loginDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okResult.Value.GetType().GetProperty("token") != null, "Response should contain a 'token' property");
            Assert.NotNull(okResult.Value.GetType().GetProperty("token").GetValue(okResult.Value, null));
        }

        [Fact]
        public async Task Login_Failure_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new UsuarioLoginDTO
            {
                Email = "test@test.com",
                Senha = "123456"
            };

            var usuario = new Usuario
            {
                IdUsuario = 1,
                Email = loginDto.Email,
                NomeUsuario = "Test User"
            };

            _mockUsuarioRepository.Setup(x => x.GetUsuarioByEmail(loginDto.Email))
                .ReturnsAsync(usuario);

            _mockUsuarioRepository.Setup(x => x.CheckPassword(loginDto))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Email ou senha inválidos", unauthorizedResult.Value);
        }
    }
}
