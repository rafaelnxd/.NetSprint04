using Xunit;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Tests.Integration
{
    public class UsuariosServiceIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateUsuarioAsync_ComEmailDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Usuario>(context);
            var service = new UsuariosService(repo);

            var usuario1 = new Usuario
            {
                Email = "duplicado@teste.com",
                Nome = "Usuário 1",
                Senha = "senha123"
            };

            var usuario2 = new Usuario
            {
                Email = "duplicado@teste.com",
                Nome = "Usuário 2",
                Senha = "senha456"
            };

            await service.CreateUsuarioAsync(usuario1);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(async () =>
            {
                await service.CreateUsuarioAsync(usuario2);
            });
        }

        [Fact]
        public async Task UpdateUsuarioAsync_DeveAtualizarDadosDoUsuario()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Usuario>(context);
            var service = new UsuariosService(repo);

            var usuario = new Usuario
            {
                Email = "atualizar@teste.com",
                Nome = "Antigo Nome",
                Senha = "123456"
            };

            await service.CreateUsuarioAsync(usuario);
            var usuarioNoBanco = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "atualizar@teste.com");

            // Modificar dados
            usuarioNoBanco.Nome = "Novo Nome";
            usuarioNoBanco.Senha = "novaSenha";

            // Act
            await service.UpdateUsuarioAsync(usuarioNoBanco);

            // Assert
            var usuarioAtualizado = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "atualizar@teste.com");
            Assert.Equal("Novo Nome", usuarioAtualizado.Nome);
            Assert.Equal("novaSenha", usuarioAtualizado.Senha);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DeveRemoverUsuarioDoBanco()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new Repository<Usuario>(context);
            var service = new UsuariosService(repo);

            var usuario = new Usuario
            {
                Email = "deletar@teste.com",
                Nome = "Usuário Deletar",
                Senha = "senha123"
            };

            await service.CreateUsuarioAsync(usuario);
            var usuarioNoBanco = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "deletar@teste.com");

            // Act
            await service.DeleteUsuarioAsync(usuarioNoBanco.UsuarioId);

            // Assert
            var usuarioDeletado = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "deletar@teste.com");
            Assert.Null(usuarioDeletado);
        }
    }
}
