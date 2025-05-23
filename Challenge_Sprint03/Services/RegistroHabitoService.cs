using Challenge_Sprint03.Models;
using Challenge_Sprint03.Models.DTOs;
using Challenge_Sprint03.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Services
{
    public class RegistroHabitoService
    {
        private readonly IRegistroHabitoRepository _registroRepository;

        public RegistroHabitoService(IRegistroHabitoRepository registroRepository)
        {
            _registroRepository = registroRepository;
        }

        public async Task<IEnumerable<RegistroHabitoResponseDTO>> GetAllAsync()
        {
            var registros = await _registroRepository.GetAllWithHabitoAsync();
            var dtoList = registros.Select(r => new RegistroHabitoResponseDTO
            {
                Id = r.Id,
                Data = r.Data,
                Observacoes = r.Observacoes,
                HabitoDescricao = r.Habito != null ? r.Habito.Descricao : null
            });
            return dtoList;
        }

        public async Task<RegistroHabito> GetByIdAsync(int id)
        {
            return await _registroRepository.GetByIdAsync(id);
        }

        public async Task CreateRegistroAsync(RegistroHabito registro)
        {
            registro.Data = DateTime.Now;
            await _registroRepository.AddAsync(registro);
        }

        public async Task UpdateRegistroAsync(RegistroHabito registro)
        {
            var registroExistente = await _registroRepository.GetByIdAsync(registro.Id);
            if (registroExistente == null)
                throw new Exception("Registro não encontrado");

            registroExistente.HabitoId = registro.HabitoId;
            registroExistente.Imagem = registro.Imagem;
            registroExistente.Observacoes = registro.Observacoes;

            await _registroRepository.UpdateAsync(registroExistente);
        }


        public async Task DeleteRegistroAsync(int id)
        {
            await _registroRepository.DeleteAsync(id);
        }
    }
}
