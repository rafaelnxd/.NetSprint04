using Xunit;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using System.Threading.Tasks;
using System.Linq;

namespace Challenge_Sprint03.Tests.Integration
{
    public class UnidadesServiceIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateUnidadeAsync_DeveSalvarUnidadeNoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Unidade>(context);
            var service = new UnidadesService(repo);

            var unidade = new Unidade
            {
                Nome = "Unidade Centro",
                Estado = "SP",
                Cidade = "São Paulo",
                Endereco = "Av. Paulista, 1000"
            };

            // Act
            await service.CreateUnidadeAsync(unidade);

            // Assert
            var unidadeNoBanco = await context.Unidades.FirstOrDefaultAsync(u => u.Nome == "Unidade Centro");
            Assert.NotNull(unidadeNoBanco);
            Assert.Equal("SP", unidadeNoBanco.Estado);
        }

        [Fact]
        public async Task UpdateUnidadeAsync_DeveAtualizarCampos()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Unidade>(context);
            var service = new UnidadesService(repo);

            var unidade = new Unidade
            {
                Nome = "Unidade Leste",
                Estado = "SP",
                Cidade = "São Paulo",
                Endereco = "Rua das Flores, 50"
            };

            await service.CreateUnidadeAsync(unidade);
            var unidadeNoBanco = await context.Unidades.FirstOrDefaultAsync(u => u.Nome == "Unidade Leste");

            unidadeNoBanco.Cidade = "Guarulhos";
            unidadeNoBanco.Endereco = "Rua Nova, 123";

            // Act
            await service.UpdateUnidadeAsync(unidadeNoBanco);

            // Assert
            var unidadeAtualizada = await context.Unidades.FirstOrDefaultAsync(u => u.UnidadeId == unidadeNoBanco.UnidadeId);
            Assert.Equal("Guarulhos", unidadeAtualizada.Cidade);
            Assert.Equal("Rua Nova, 123", unidadeAtualizada.Endereco);
        }

        [Fact]
        public async Task DeleteUnidadeAsync_DeveRemoverUnidadeDoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Unidade>(context);
            var service = new UnidadesService(repo);

            var unidade = new Unidade
            {
                Nome = "Unidade Sul",
                Estado = "SP",
                Cidade = "Santo André",
                Endereco = "Rua do Comércio, 200"
            };

            await service.CreateUnidadeAsync(unidade);
            var unidadeNoBanco = await context.Unidades.FirstOrDefaultAsync(u => u.Nome == "Unidade Sul");

            // Act
            await service.DeleteUnidadeAsync(unidadeNoBanco.UnidadeId);

            // Assert
            var unidadeDeletada = await context.Unidades.FirstOrDefaultAsync(u => u.UnidadeId == unidadeNoBanco.UnidadeId);
            Assert.Null(unidadeDeletada);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarUnidadeCorreta()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Unidade>(context);
            var service = new UnidadesService(repo);

            var unidade = new Unidade
            {
                Nome = "Unidade Norte",
                Estado = "SP",
                Cidade = "São Paulo",
                Endereco = "Av. Norte, 321"
            };

            await service.CreateUnidadeAsync(unidade);
            var unidadeNoBanco = await context.Unidades.FirstOrDefaultAsync(u => u.Nome == "Unidade Norte");

            // Act
            var resultado = await service.GetByIdAsync(unidadeNoBanco.UnidadeId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Unidade Norte", resultado.Nome);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodasUnidades()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Unidade>(context);
            var service = new UnidadesService(repo);

            await service.CreateUnidadeAsync(new Unidade { Nome = "Unidade A", Estado = "SP", Cidade = "Campinas", Endereco = "Rua A, 1" });
            await service.CreateUnidadeAsync(new Unidade { Nome = "Unidade B", Estado = "SP", Cidade = "Sorocaba", Endereco = "Rua B, 2" });

            // Act
            var unidades = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, unidades.Count());
            Assert.Contains(unidades, u => u.Nome == "Unidade A");
            Assert.Contains(unidades, u => u.Cidade == "Sorocaba");
        }
    }
}
