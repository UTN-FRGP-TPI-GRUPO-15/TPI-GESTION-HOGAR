using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //DbSets
        public DbSet<Hijo> Hijos { get; set; }
        public DbSet<Mujer> mujeres { get; set; }
    }
}
