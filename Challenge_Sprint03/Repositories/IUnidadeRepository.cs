using Challenge_Sprint03.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Repositories
{
    public interface IUnidadeRepository : IRepository<Unidade>
    {
        Task<IEnumerable<Unidade>> GetAllWithDetailsAsync();
    }
}
