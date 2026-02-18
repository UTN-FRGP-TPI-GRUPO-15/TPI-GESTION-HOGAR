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
        public DbSet<Agresor> Agresores { get; set; }
        public DbSet<Denuncia> Denuncias { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Hijo> Hijos { get; set; }
        public DbSet<Medida> Medidas { get; set; }
        public DbSet<Mujer> Mujeres { get; set; }
        public DbSet<Observacion> Observaciones { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<Condicion> Condiciones { get; set; }
        public DbSet<TipoCondicion> TipoCondiciones { get; set; }
        public DbSet<ObservacionCondicion> ObservacionCondiciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<TipoTurno> TipoTurnos { get; set; }

        
    }
}
