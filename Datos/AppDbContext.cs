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




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // 1. TABLAS CATÁLOGO (Configuraciones base)
            // =========================================================

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Descripcion = "Administradora" },
                new Rol { Id = 2, Descripcion = "Equipo Técnico" },
                new Rol { Id = 3, Descripcion = "Operadora" }
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
                new Habitacion { Id = 1, NroHabitacion = 1, Capacidad = 4, Estado = true },
                new Habitacion { Id = 2, NroHabitacion = 2, Capacidad = 5, Estado = true },
                new Habitacion { Id = 3, NroHabitacion = 3, Capacidad = 6, Estado = true },
                new Habitacion { Id = 4, NroHabitacion = 4, Capacidad = 2, Estado = true }
            );

            // =========================================================
            // 2. PERSONAL Y USUARIOS
            // =========================================================

            
            modelBuilder.Entity<Personal>().HasData(
                new Personal
                {
                    ID = 1,
                    Legajo = 1001,
                    Apellido = "García",
                    Nombre = "Laura",
                    DNI = 25123456,
                    Nacionalidad = "Argentina",
                    FechaNac = new DateOnly(1980, 5, 12),
                    Telefono = "2246-112233",
                    Domicilio = "Calle 4 Nro 123",
                    Localidad = "San Clemente del Tuyú",
                    estado = true
                }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreUsuario = "lgarcia",
                    Clave = "123456",
                    PersonalId = 1,
                    RolId = 2
                }
            );

            // =========================================================
            // 3. CASO DE PRUEBA (Mujer, Hijos, Registro, Agresor, Salud)
            // =========================================================

            modelBuilder.Entity<Mujer>().HasData(
                new Mujer
                {
                    ID = 1,
                    DNI = 30111222,
                    Apellido = "López",
                    Nombre = "María",
                    Nacionalidad = "Argentina",
                    FechaNac = new DateOnly(1985, 10, 20),
                    Genero = "Femenino",
                    NivelEducativo = "Secundario Completo",
                    Ocupacion = "Empleada de Comercio",
                    Telefono = "2246-554433",
                    Domicilio = "Av. San Martín 456",
                    Localidad = "Mar de Ajó",
                    estado = true
                }
            );

            // Ejemplo de cómo registraríamos que María requiere medicación para el asma
            modelBuilder.Entity<Condicion>().HasData(
                new Condicion
                {
                    Id = 1,
                    MujerId = 1,
                    TipoCondicionId = 1, // Enfermedad Crónica
                    ObservacionCondicionId = 1 // En Tratamiento Médico
                }
            );

            modelBuilder.Entity<Hijo>().HasData(
                new Hijo
                {
                    ID = 1,
                    DNI = 50333444,
                    Apellido = "Pérez López",
                    Nombre = "Mateo",
                    FechaNac = new DateOnly(2015, 3, 15),
                    MujerId = 1
                }
            );

            modelBuilder.Entity<Registro>().HasData(
                new Registro
                {
                    Id = 1,
                    Fecha = new DateOnly(2026, 2, 10),
                    Estado = true,
                    MujerID = 1,
                    HabitacionId = 1
                }
            );

            modelBuilder.Entity<Agresor>().HasData(
                new Agresor
                {
                    ID = 1,
                    DNI = 29888777,
                    Apellido = "Pérez",
                    Nombre = "Carlos",
                    Nacionalidad = "Argentina",
                    FechaNac = new DateOnly(1982, 8, 5),
                    Genero = "Masculino",
                    NivelEducativo = "Primario",
                    Ocupacion = "Albañil",
                    Telefono = "2246-998877",
                    Domicilio = "Av. San Martín 456",
                    Localidad = "Mar de Ajó",
                    RegistroId = 1
                }
            );

            modelBuilder.Entity<Denuncia>().HasData(
                new Denuncia
                {
                    ID = 1,
                    Fecha = new DateOnly(2026, 2, 9),
                    NroIPP = 12345,
                    NroExp = 67890,
                    RegistroId = 1
                }
            );

            modelBuilder.Entity<Medida>().HasData(
                new Medida { Id = 1, Descripcion = "Perimetral 300 metros", DenunciaId = 1 },
                new Medida { Id = 2, Descripcion = "Entrega de botón antipánico", DenunciaId = 1 }
            );
        }
    }
}
    
   

    
