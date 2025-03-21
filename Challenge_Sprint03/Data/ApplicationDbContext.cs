using Microsoft.EntityFrameworkCore;
using Challenge_Sprint03.Models;

namespace Challenge_Sprint03.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Habito> Habitos { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RegistroHabito> RegistrosHabito { get; set; }
    }
}
