using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllWithDetailsAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}
