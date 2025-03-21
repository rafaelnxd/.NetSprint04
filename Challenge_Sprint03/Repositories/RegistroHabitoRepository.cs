using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Repositories
{
    public class RegistroHabitoRepository : Repository<RegistroHabito>, IRegistroHabitoRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistroHabitoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistroHabito>> GetAllWithHabitoAsync()
        {
            return await _context.RegistrosHabito.Include(r => r.Habito).ToListAsync();
        }
    }
}
