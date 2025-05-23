using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;

namespace Challenge_Sprint03.Tests.Services
{
	public class RegistroHabitoServiceUnitTests
	{
		private readonly Mock<IRegistroHabitoRepository> _registroRepositoryMock;
		private readonly RegistroHabitoService _service;

		public RegistroHabitoServiceUnitTests()
		{
			_registroRepositoryMock = new Mock<IRegistroHabitoRepository>();
			_service = new RegistroHabitoService(_registroRepositoryMock.Object);
		}

		[Fact]
		public async Task GetAllAsync_DeveRetornarListaDeDTOs()
		{
			// Arrange
			var registros = new List<RegistroHabito>
			{
				new RegistroHabito { Id = 1, Data = DateTime.Now, Observacoes = "Obs1", Habito = new Habito { Descricao = "Hábito 1" } },
				new RegistroHabito { Id = 2, Data = DateTime.Now, Observacoes = "Obs2", Habito = new Habito { Descricao = "Hábito 2" } }
			};

			_registroRepositoryMock.Setup(r => r.GetAllWithHabitoAsync()).ReturnsAsync(registros);

			// Act
			var result = await _service.GetAllAsync();

			// Assert
			Assert.Equal(2, result.Count());
			Assert.Contains(result, r => r.Observacoes == "Obs1");
			Assert.Contains(result, r => r.HabitoDescricao == "Hábito 2");
		}

		[Fact]
		public async Task GetByIdAsync_DeveRetornarRegistroCorreto()
		{
			// Arrange
			var registro = new RegistroHabito { Id = 1, Observacoes = "Teste" };
			_registroRepositoryMock.Setup(r => r.GetByIdAsync(registro.Id)).ReturnsAsync(registro);

			// Act
			var result = await _service.GetByIdAsync(registro.Id);

			// Assert
			Assert.Equal("Teste", result.Observacoes);
		}

		[Fact]
		public async Task CreateRegistroAsync_DeveChamarAddAsyncEAtualizarData()
		{
			// Arrange
			var registro = new RegistroHabito { Observacoes = "Novo registro" };

			// Act
			await _service.CreateRegistroAsync(registro);

			// Assert
			Assert.NotEqual(default, registro.Data);
			_registroRepositoryMock.Verify(r => r.AddAsync(registro), Times.Once);
		}

		[Fact]
		public async Task UpdateRegistroAsync_RegistroNaoEncontrado_DeveLancarException()
		{
			// Arrange
			var registro = new RegistroHabito { Id = 1 };
			_registroRepositoryMock.Setup(r => r.GetByIdAsync(registro.Id)).ReturnsAsync((RegistroHabito)null);

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => _service.UpdateRegistroAsync(registro));
		}

		[Fact]
		public async Task UpdateRegistroAsync_DeveChamarUpdateAsync()
		{
			// Arrange
			var registro = new RegistroHabito { Id = 1, HabitoId = 2, Imagem = "img.png", Observacoes = "Atualizado" };
			var registroExistente = new RegistroHabito { Id = 1 };

			_registroRepositoryMock.Setup(r => r.GetByIdAsync(registro.Id)).ReturnsAsync(registroExistente);

			// Act
			await _service.UpdateRegistroAsync(registro);

			// Assert
			_registroRepositoryMock.Verify(r => r.UpdateAsync(It.Is<RegistroHabito>(r => r.Observacoes == "Atualizado")), Times.Once);
		}

		[Fact]
		public async Task DeleteRegistroAsync_DeveChamarDeleteAsync()
		{
			// Arrange
			var registroId = 1;

			// Act
			await _service.DeleteRegistroAsync(registroId);

			// Assert
			_registroRepositoryMock.Verify(r => r.DeleteAsync(registroId), Times.Once);
		}
	}
}
