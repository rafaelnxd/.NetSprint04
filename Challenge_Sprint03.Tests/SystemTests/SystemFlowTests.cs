using Xunit;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using System.Threading.Tasks;
using System.Linq;

namespace Challenge_Sprint03.Tests.SystemTests
{
    public class SystemFlowTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SystemTestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task FluxoCompleto_HabitoDentario_DeveExecutarComSucesso()
        {
            // Arrange
            var context = GetDbContext();

            var usuarioRepo = new Repository<Usuario>(context);
            var habitoRepo = new Repository<Habito>(context);
            var registroRepo = new RegistroHabitoRepository(context);

            var usuarioService = new UsuariosService(usuarioRepo);
            var habitoService = new HabitoService(habitoRepo);
            var registroService = new RegistroHabitoService(registroRepo);

            // 1 - Criar um Usuário
            var usuario = new Usuario { Nome = "João", Email = "joao@saudeoral.com", Senha = "senha123" };
            await usuarioService.CreateUsuarioAsync(usuario);

            // 2 - Criar um Hábito Dentário
            var habito = new Habito { Descricao = "Escovar os dentes", Tipo = "Higiene Oral", FrequenciaIdeal = 3 };
            await habitoService.CreateHabitoAsync(habito);

            // 3 - Criar um Registro de Hábito
            var registro = new RegistroHabito
            {
                HabitoId = habito.HabitoId,
                Observacoes = "Escovei 3x hoje",
                Imagem = "escovacao_dia.png"
            };
            await registroService.CreateRegistroAsync(registro);

            // 4 - Recuperar o Registro e validar
            var registros = await registroService.GetAllAsync();
            var registroRecuperado = registros.FirstOrDefault(r => r.Observacoes == "Escovei 3x hoje");

            // Assert
            Assert.NotNull(registroRecuperado);
            Assert.Equal("Escovar os dentes", registroRecuperado.HabitoDescricao);
            Assert.Equal("Escovei 3x hoje", registroRecuperado.Observacoes);
        }
    }
}
