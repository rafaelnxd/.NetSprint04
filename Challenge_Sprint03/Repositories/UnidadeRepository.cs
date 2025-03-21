using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Repositories
{
    public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
    {
        private readonly ApplicationDbContext _context;

        public UnidadeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Unidade>> GetAllWithDetailsAsync()
        {
            return await _context.Unidades.ToListAsync();
        }
    }
}
