using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;

namespace Challenge_Sprint03.Tests.Services
{
    public class UnidadesServiceUnitTests
    {
        private readonly Mock<IRepository<Unidade>> _unidadeRepositoryMock;
        private readonly UnidadesService _service;

        public UnidadesServiceUnitTests()
        {
            _unidadeRepositoryMock = new Mock<IRepository<Unidade>>();
            _service = new UnidadesService(_unidadeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarUnidades()
        {
            // Arrange
            var unidades = new List<Unidade>
            {
                new Unidade { UnidadeId = 1, Nome = "Unidade 1" },
                new Unidade { UnidadeId = 2, Nome = "Unidade 2" }
            };

            _unidadeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(unidades);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarUnidadeCorreta()
        {
            // Arrange
            var unidade = new Unidade { UnidadeId = 1, Nome = "Unidade Teste" };
            _unidadeRepositoryMock.Setup(r => r.GetByIdAsync(unidade.UnidadeId)).ReturnsAsync(unidade);

            // Act
            var result = await _service.GetByIdAsync(unidade.UnidadeId);

            // Assert
            Assert.Equal("Unidade Teste", result.Nome);
        }

        [Fact]
        public async Task CreateUnidadeAsync_DeveChamarAddAsync()
        {
            // Arrange
            var unidade = new Unidade { Nome = "Nova Unidade" };

            // Act
            await _service.CreateUnidadeAsync(unidade);

            // Assert
            _unidadeRepositoryMock.Verify(r => r.AddAsync(unidade), Times.Once);
        }

        [Fact]
        public async Task UpdateUnidadeAsync_UnidadeNaoEncontrada_DeveLancarException()
        {
            // Arrange
            var unidade = new Unidade { UnidadeId = 1, Nome = "Atualizar" };
            _unidadeRepositoryMock.Setup(r => r.GetByIdAsync(unidade.UnidadeId)).ReturnsAsync((Unidade)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateUnidadeAsync(unidade));
        }

        [Fact]
        public async Task UpdateUnidadeAsync_DeveChamarUpdateAsync()
        {
            // Arrange
            var unidade = new Unidade { UnidadeId = 1, Nome = "Atualizar", Estado = "SP", Cidade = "São Paulo", Endereco = "Rua X" };
            _unidadeRepositoryMock.Setup(r => r.GetByIdAsync(unidade.UnidadeId)).ReturnsAsync(new Unidade());

            // Act
            await _service.UpdateUnidadeAsync(unidade);

            // Assert
            _unidadeRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Unidade>(u => u.Nome == "Atualizar")), Times.Once);
        }

        [Fact]
        public async Task DeleteUnidadeAsync_DeveChamarDeleteAsync()
        {
            // Arrange
            var unidadeId = 1;

            // Act
            await _service.DeleteUnidadeAsync(unidadeId);

            // Assert
            _unidadeRepositoryMock.Verify(r => r.DeleteAsync(unidadeId), Times.Once);
        }
    }
}
