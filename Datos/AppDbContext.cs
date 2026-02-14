using Microsoft.EntityFrameworkCore;

namespace TPI_GESTION_HOGAR.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
    }
}
