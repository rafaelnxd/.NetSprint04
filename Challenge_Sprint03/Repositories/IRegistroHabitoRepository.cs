using Challenge_Sprint03.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Repositories
{
    public interface IRegistroHabitoRepository : IRepository<RegistroHabito>
    {
        Task<IEnumerable<RegistroHabito>> GetAllWithHabitoAsync();
    }
}
