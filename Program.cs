using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

var builder = WebApplication.CreateBuilder(args);

//Cofiguramos cadena de concexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Crear base si no existe y/o aplicar migraciones pendientes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    //Acá se puede hacer seed de registros necesarios por defecto (por ejemplo roles de usuario, usuarios de prueba, mujeres de prueba, etc.)
    if(db.Usuarios.FirstOrDefault(u => u.NombreUsuario == "admin") == null)
    {
        PasswordHasher<Usuario> hasher = new();

        Personal personalAdmin = new Personal
        {
            Legajo = 0,
            Apellido = "Apellido",
            Nombre = "Nombre",
            DNI = 0,
            FechaNac = new DateOnly(2000, 1, 1),
            estado = true,
            Nacionalidad = "Nacionalidad"
        };

        db.Personal.Add(personalAdmin);

        db.Usuarios.Add(
            new Usuario { NombreUsuario = "admin", Clave = hasher.HashPassword(null!, "admin"), Personal = personalAdmin, RolId = 1}
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
