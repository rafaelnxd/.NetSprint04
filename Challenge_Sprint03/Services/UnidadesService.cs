using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Services
{
    public class UnidadesService
    {
        private readonly IRepository<Unidade> _unidadeRepository;

        public UnidadesService(IRepository<Unidade> unidadeRepository)
        {
            _unidadeRepository = unidadeRepository;
        }

        public async Task<IEnumerable<Unidade>> GetAllAsync()
        {
            return await _unidadeRepository.GetAllAsync();
        }

        public async Task<Unidade> GetByIdAsync(int id)
        {
            return await _unidadeRepository.GetByIdAsync(id);
        }

        public async Task CreateUnidadeAsync(Unidade unidade)
        {
            await _unidadeRepository.AddAsync(unidade);
        }

        public async Task UpdateUnidadeAsync(Unidade unidade)
        {
            var unidadeExistente = await _unidadeRepository.GetByIdAsync(unidade.UnidadeId);
            if (unidadeExistente == null)
                throw new Exception("Unidade não encontrada");

            // Atualiza os campos da unidade
            unidadeExistente.Nome = unidade.Nome;
            unidadeExistente.Estado = unidade.Estado;
            unidadeExistente.Cidade = unidade.Cidade;
            unidadeExistente.Endereco = unidade.Endereco;

            await _unidadeRepository.UpdateAsync(unidadeExistente);
        }

        public async Task DeleteUnidadeAsync(int id)
        {
            await _unidadeRepository.DeleteAsync(id);
        }
    }
}
