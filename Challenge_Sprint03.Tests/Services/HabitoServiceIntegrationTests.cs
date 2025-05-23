using Xunit;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Tests.Integration
{
    public class HabitoServiceIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateHabitoAsync_DeveSalvarHabitoNoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Habito>(context);
            var service = new HabitoService(repo);

            var habito = new Habito
            {
                Descricao = "Ler 10 p�ginas",
                Tipo = "Educa��o",
                FrequenciaIdeal = 1
            };

            // Act
            await service.CreateHabitoAsync(habito);

            // Assert
            var habitoNoBanco = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Ler 10 p�ginas");
            Assert.NotNull(habitoNoBanco);
            Assert.Equal("Educa��o", habitoNoBanco.Tipo);
        }

        [Fact]
        public async Task UpdateHabitoAsync_DeveAtualizarHabito()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Habito>(context);
            var service = new HabitoService(repo);

            var habito = new Habito
            {
                Descricao = "Exerc�cio f�sico",
                Tipo = "Sa�de",
                FrequenciaIdeal = 3
            };

            await service.CreateHabitoAsync(habito);
            var habitoNoBanco = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Exerc�cio f�sico");

            habitoNoBanco.FrequenciaIdeal = 5;

            // Act
            await service.UpdateHabitoAsync(habitoNoBanco);

            // Assert
            var habitoAtualizado = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Exerc�cio f�sico");
            Assert.Equal(5, habitoAtualizado.FrequenciaIdeal);
        }

        [Fact]
        public async Task DeleteHabitoAsync_DeveRemoverHabitoDoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Habito>(context);
            var service = new HabitoService(repo);

            var habito = new Habito
            {
                Descricao = "Meditar",
                Tipo = "Bem-estar",
                FrequenciaIdeal = 1
            };

            await service.CreateHabitoAsync(habito);
            var habitoNoBanco = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Meditar");

            // Act
            await service.DeleteHabitoAsync(habitoNoBanco.HabitoId);

            // Assert
            var habitoDeletado = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Meditar");
            Assert.Null(habitoDeletado);
        }

        [Fact]
        public async Task GetHabitoByIdAsync_DeveRetornarHabitoCorreto()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Habito>(context);
            var service = new HabitoService(repo);

            var habito = new Habito
            {
                Descricao = "Dormir bem",
                Tipo = "Sa�de",
                FrequenciaIdeal = 1
            };

            await service.CreateHabitoAsync(habito);
            var habitoNoBanco = await context.Habitos.FirstOrDefaultAsync(h => h.Descricao == "Dormir bem");

            // Act
            var resultado = await service.GetHabitoByIdAsync(habitoNoBanco.HabitoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Dormir bem", resultado.Descricao);
        }

        [Fact]
        public async Task GetAllHabitosAsync_DeveRetornarTodosOsHabitos()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Habito>(context);
            var service = new HabitoService(repo);

            await service.CreateHabitoAsync(new Habito { Descricao = "Beber �gua", Tipo = "Sa�de", FrequenciaIdeal = 8 });
            await service.CreateHabitoAsync(new Habito { Descricao = "Estudar", Tipo = "Educa��o", FrequenciaIdeal = 2 });

            // Act
            var habitos = await service.GetAllHabitosAsync();

            // Assert
            Assert.Equal(2, System.Linq.Enumerable.Count(habitos));
        }
    }
}
