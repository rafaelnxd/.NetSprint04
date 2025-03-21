using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Services
{
    public class HabitoService
    {
        private readonly IRepository<Habito> _habitoRepository;

        public HabitoService(IRepository<Habito> habitoRepository)
        {
            _habitoRepository = habitoRepository;
        }

        public async Task<IEnumerable<Habito>> GetAllHabitosAsync()
        {
            return await _habitoRepository.GetAllAsync();
        }

        public async Task<Habito> GetHabitoByIdAsync(int id)
        {
            return await _habitoRepository.GetByIdAsync(id);
        }

        public async Task CreateHabitoAsync(Habito habito)
        {
            await _habitoRepository.AddAsync(habito);
        }

        public async Task UpdateHabitoAsync(Habito habito)
        {
            await _habitoRepository.UpdateAsync(habito);
        }

        public async Task DeleteHabitoAsync(int id)
        {
            await _habitoRepository.DeleteAsync(id);
        }
    }
}
