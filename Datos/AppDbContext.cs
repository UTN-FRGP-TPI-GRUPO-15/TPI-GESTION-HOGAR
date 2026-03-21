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
        public DbSet<TipoMedida> TiposMedidas { get; set; }
        public DbSet<Mujer> Mujeres { get; set; }
        public DbSet<Observacion> Observaciones { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<Recordatorio> Recordatorios{ get; set; }
        public DbSet<Condicion> Condiciones { get; set; }
        public DbSet<TipoCondicion> TipoCondiciones { get; set; }
        public DbSet<ObservacionCondicion> ObservacionCondiciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Novedad> Novedades { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<TipoTurno> TipoTurnos { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
        new Rol { Id = 1, Descripcion = "Equipo Técnico" },
        new Rol { Id = 2, Descripcion = "Operadora" }
    );

            modelBuilder.Entity<TipoTurno>().HasData(
                new TipoTurno { Id = 1, Descripcion = "Mañana (08:00 a 14:00)" },
                new TipoTurno { Id = 2, Descripcion = "Tarde (14:00 a 20:00)" },
                new TipoTurno { Id = 3, Descripcion = "Noche (20:00 a 08:00)" }
            );

            modelBuilder.Entity<TipoCondicion>().HasData(
                new TipoCondicion { Id = 1, Descripcion = "Enfermedad Crónica" },
                new TipoCondicion { Id = 2, Descripcion = "Discapacidad Física" },
                new TipoCondicion { Id = 3, Descripcion = "Discapacidad Intelectual/Mental" },
                new TipoCondicion { Id = 4, Descripcion = "Adicción (Consumo Problemático)" },
                new TipoCondicion { Id = 5, Descripcion = "Otra Condición Médica" }
            );

            modelBuilder.Entity<ObservacionCondicion>().HasData(
                new ObservacionCondicion { Id = 1, Descripcion = "En Tratamiento Médico" },
                new ObservacionCondicion { Id = 2, Descripcion = "Sin Tratamiento Actual" },
                new ObservacionCondicion { Id = 3, Descripcion = "Requiere Atención/Derivación" }
            );

            modelBuilder.Entity<Habitacion>().HasData(
                new Habitacion { Id = 1, NroHabitacion = 1, Capacidad = 5, Estado = true },
                new Habitacion { Id = 2, NroHabitacion = 2, Capacidad = 5, Estado = true },
                new Habitacion { Id = 3, NroHabitacion = 3, Capacidad = 5, Estado = true },
                new Habitacion { Id = 4, NroHabitacion = 4, Capacidad = 5, Estado = true },
                new Habitacion { Id = 5, NroHabitacion = 5, Capacidad = 5, Estado = true },
                new Habitacion { Id = 6, NroHabitacion = 6, Capacidad = 5, Estado = true }
            );

            modelBuilder.Entity<TipoMedida>().HasData(
                new TipoMedida { Id = 1, Descripcion = "Prohibición de acercamiento (Perimetral)" },
                new TipoMedida { Id = 2, Descripcion = "Exclusión del hogar del agresor" },
                new TipoMedida { Id = 3, Descripcion = "Entrega de Botón Antipánico / Aplicación Móvil" },
                new TipoMedida { Id = 4, Descripcion = "Cese de actos de perturbación / hostigamiento" },
                new TipoMedida { Id = 5, Descripcion = "Restitución de efectos personales" },
                new TipoMedida { Id = 6, Descripcion = "Ronda policial periódica en el domicilio" }
            );


        }
    }
}



