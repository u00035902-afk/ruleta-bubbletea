using Microsoft.EntityFrameworkCore;

namespace RuletanBublee.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<GiroResultado> GirosResultados { get; set; }
    }
}
