using Xunit;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using System.Threading.Tasks;
using System.Linq;

namespace Challenge_Sprint03.Tests.Integration
{
    public class RegistroHabitoServiceIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateRegistroAsync_DeveSalvarRegistroComDataAtual()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new RegistroHabitoRepository(context);
            var service = new RegistroHabitoService(repo);

            var habito = new Habito { Descricao = "Beber água", Tipo = "Saúde", FrequenciaIdeal = 8 };
            context.Habitos.Add(habito);
            await context.SaveChangesAsync();

            var registro = new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "Bebi 1L", Imagem = "teste.png" };

            // Act
            await service.CreateRegistroAsync(registro);

            // Assert
            var registroNoBanco = await context.RegistrosHabito.FirstOrDefaultAsync(r => r.Observacoes == "Bebi 1L");
            Assert.NotNull(registroNoBanco);
            Assert.NotEqual(default, registroNoBanco.Data);
        }

        [Fact]
        public async Task UpdateRegistroAsync_DeveAtualizarObservacoes()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new RegistroHabitoRepository(context);
            var service = new RegistroHabitoService(repo);

            var habito = new Habito { Descricao = "Meditar", Tipo = "Bem-estar", FrequenciaIdeal = 1 };
            context.Habitos.Add(habito);
            await context.SaveChangesAsync();

            var registro = new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "Antes de dormir", Imagem = "teste.png" };
            context.RegistrosHabito.Add(registro);
            await context.SaveChangesAsync();

            registro.Observacoes = "De manhã";

            // Act
            await service.UpdateRegistroAsync(registro);

            // Assert
            var registroAtualizado = await context.RegistrosHabito.FirstOrDefaultAsync(r => r.Id == registro.Id);
            Assert.Equal("De manhã", registroAtualizado.Observacoes);
        }

        [Fact]
        public async Task DeleteRegistroAsync_DeveRemoverRegistroDoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new RegistroHabitoRepository(context);
            var service = new RegistroHabitoService(repo);

            var habito = new Habito { Descricao = "Caminhar", Tipo = "Saúde", FrequenciaIdeal = 3 };
            context.Habitos.Add(habito);
            await context.SaveChangesAsync();

            var registro = new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "5km", Imagem = "teste.png" };
            context.RegistrosHabito.Add(registro);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteRegistroAsync(registro.Id);

            // Assert
            var registroDeletado = await context.RegistrosHabito.FirstOrDefaultAsync(r => r.Id == registro.Id);
            Assert.Null(registroDeletado);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarRegistroCorreto()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new RegistroHabitoRepository(context);
            var service = new RegistroHabitoService(repo);

            var habito = new Habito { Descricao = "Leitura", Tipo = "Educação", FrequenciaIdeal = 1 };
            context.Habitos.Add(habito);
            await context.SaveChangesAsync();

            var registro = new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "Li 20 páginas", Imagem = "teste.png" };
            context.RegistrosHabito.Add(registro);
            await context.SaveChangesAsync();

            // Act
            var resultado = await service.GetByIdAsync(registro.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Li 20 páginas", resultado.Observacoes);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarDTOsCorretos()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new RegistroHabitoRepository(context);
            var service = new RegistroHabitoService(repo);

            var habito = new Habito { Descricao = "Exercício", Tipo = "Saúde", FrequenciaIdeal = 3 };
            context.Habitos.Add(habito);
            await context.SaveChangesAsync();

            context.RegistrosHabito.Add(new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "Corrida", Imagem = "teste.png" });
            context.RegistrosHabito.Add(new RegistroHabito { HabitoId = habito.HabitoId, Observacoes = "Natação", Imagem = "teste.png" });
            await context.SaveChangesAsync();

            // Act
            var dtos = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, dtos.Count());
            Assert.Contains(dtos, d => d.Observacoes == "Corrida");
            Assert.Contains(dtos, d => d.HabitoDescricao == "Exercício");
        }
    }
}
