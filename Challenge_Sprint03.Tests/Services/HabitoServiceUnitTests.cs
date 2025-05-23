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
    public class HabitoServiceUnitTests
    {
        private readonly Mock<IRepository<Habito>> _habitoRepositoryMock;
        private readonly HabitoService _service;

        public HabitoServiceUnitTests()
        {
            _habitoRepositoryMock = new Mock<IRepository<Habito>>();
            _service = new HabitoService(_habitoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllHabitosAsync_DeveRetornarHabitos()
        {
            // Arrange
            var habitos = new List<Habito>
            {
                new Habito { HabitoId = 1, Descricao = "Beber Água" },
                new Habito { HabitoId = 2, Descricao = "Meditar" }
            };

            _habitoRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(habitos);

            // Act
            var result = await _service.GetAllHabitosAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetHabitoByIdAsync_DeveRetornarHabitoCorreto()
        {
            // Arrange
            var habito = new Habito { HabitoId = 1, Descricao = "Exercício" };
            _habitoRepositoryMock.Setup(r => r.GetByIdAsync(habito.HabitoId)).ReturnsAsync(habito);

            // Act
            var result = await _service.GetHabitoByIdAsync(habito.HabitoId);

            // Assert
            Assert.Equal("Exercício", result.Descricao);
        }

        [Fact]
        public async Task CreateHabitoAsync_DeveChamarAddAsync()
        {
            // Arrange
            var habito = new Habito { Descricao = "Ler" };

            // Act
            await _service.CreateHabitoAsync(habito);

            // Assert
            _habitoRepositoryMock.Verify(r => r.AddAsync(habito), Times.Once);
        }

        [Fact]
        public async Task UpdateHabitoAsync_DeveChamarUpdateAsync()
        {
            // Arrange
            var habito = new Habito { HabitoId = 1, Descricao = "Dormir bem" };

            // Act
            await _service.UpdateHabitoAsync(habito);

            // Assert
            _habitoRepositoryMock.Verify(r => r.UpdateAsync(habito), Times.Once);
        }

        [Fact]
        public async Task DeleteHabitoAsync_DeveChamarDeleteAsync()
        {
            // Arrange
            var habitoId = 1;

            // Act
            await _service.DeleteHabitoAsync(habitoId);

            // Assert
            _habitoRepositoryMock.Verify(r => r.DeleteAsync(habitoId), Times.Once);
        }
    }
}
