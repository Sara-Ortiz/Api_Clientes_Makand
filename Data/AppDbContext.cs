using Microsoft.EntityFrameworkCore;
using MaquinariaApi.Models;

namespace MaquinariaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
    }
}
