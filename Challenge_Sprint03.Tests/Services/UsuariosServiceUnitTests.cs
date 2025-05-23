using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;

namespace Challenge_Sprint03.Tests.Services
{
    public class UsuariosServiceUnitTests
    {
        private readonly Mock<IRepository<Usuario>> _usuarioRepositoryMock;
        private readonly UsuariosService _service;

        public UsuariosServiceUnitTests()
        {
            _usuarioRepositoryMock = new Mock<IRepository<Usuario>>();
            _service = new UsuariosService(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUsuarioAsync_ComEmailDuplicado_DeveLancarException()
        {
            // Arrange
            var usuario = new Usuario { Email = "teste@exemplo.com" };
            _usuarioRepositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Usuario, bool>>>()))
                .ReturnsAsync(new List<Usuario> { new Usuario { Email = "teste@exemplo.com" } });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateUsuarioAsync(usuario));
            Assert.Equal("Email já cadastrado", ex.Message);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_UsuarioNaoEncontrado_DeveLancarException()
        {
            // Arrange
            var usuario = new Usuario { UsuarioId = 1 };
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(usuario.UsuarioId)).ReturnsAsync((Usuario?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateUsuarioAsync(usuario));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public async Task UpdatePontosAsync_UsuarioNaoEncontrado_DeveLancarException()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Usuario?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdatePontosAsync(1, 10));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public async Task UpdatePontosAsync_DeveSomarPontos()
        {
            // Arrange
            var usuario = new Usuario { UsuarioId = 1, PontosRecompensa = 5 };
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(usuario.UsuarioId)).ReturnsAsync(usuario);

            // Act
            await _service.UpdatePontosAsync(usuario.UsuarioId, 10);

            // Assert
            Assert.Equal(15, usuario.PontosRecompensa);
            _usuarioRepositoryMock.Verify(r => r.UpdateAsync(usuario), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarUsuarios()
        {
            // Arrange
            var usuarios = new List<Usuario> { new Usuario { Nome = "Usuario 1" }, new Usuario { Nome = "Usuario 2" } };
            _usuarioRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(usuarios);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarUsuario()
        {
            // Arrange
            var usuario = new Usuario { UsuarioId = 1, Nome = "Teste" };
            _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(usuario.UsuarioId)).ReturnsAsync(usuario);

            // Act
            var result = await _service.GetByIdAsync(usuario.UsuarioId);

            // Assert
            Assert.Equal("Teste", result.Nome);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DeveChamarDeleteAsync()
        {
            // Arrange
            var usuarioId = 1;

            // Act
            await _service.DeleteUsuarioAsync(usuarioId);

            // Assert
            _usuarioRepositoryMock.Verify(r => r.DeleteAsync(usuarioId), Times.Once);
        }
    }
}
