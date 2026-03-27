# Sistema de Gestión para Hogar de Protección

Sistema web de gestión integral para hogares de protección de mujeres en situación de violencia. Permite administrar el ingreso y egreso de residentes, la gestión de turnos del personal operativo y la elaboración de reportes estadísticos, entre otras funcionalidades.

Proyecto Final Grupal — Tecnicatura Universitaria en Programación
Universidad Tecnológica Nacional · Facultad Regional General Pacheco

---

## Integrantes

- Tomás Ezequiel Martínez
- Rodrigo Meren
- Elian Varela

---

## Funcionalidades principales

- **Gestión de residentes**: registro de mujeres e hijos/as a cargo, con seguimiento de su situación dentro del hogar.
- **Control de ingresos y egresos**: registro completo del ciclo de permanencia en el hogar.
- **Gestión de personal y turnos**: administración del plantel operativo y asignación de turnos de trabajo.
- **Seguimiento de casos**: vinculación con agresores, medidas judiciales y denuncias asociadas.
- **Habitaciones**: control de disponibilidad y asignación de habitaciones.
- **Novedades**: registro de novedades y eventos relevantes del hogar.
- **Recordatorios**: sistema de recordatorios para el personal.
- **Reportes estadísticos**: generación de informes sobre la actividad del hogar.
- **Recuperación de contraseña**: envío de enlace de restablecimiento por email.

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| Framework web | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 10 |
| Base de datos | SQL Server |
| Autenticación | Cookie-based (ASP.NET Core) |
| Email | MailKit + Gmail SMTP |
| Contenedores | Docker |
| CI/CD | GitHub Actions |

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (o SQL Server Express)
- Git

---

## Configuración local

### 1. Clonar el repositorio

```bash
git clone https://github.com/UTN-FRGP-TPI-GRUPO-15/TPI-GESTION-HOGAR.git
cd TPI-GESTION-HOGAR
```

### 2. Configurar la cadena de conexión

Editar `appsettings.Development.json` con los datos del servidor local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=HogarDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Configurar credenciales de email (User Secrets)

Las credenciales **no deben ir en archivos del repositorio**. Usar el gestor de secretos de .NET:

```bash
dotnet user-secrets init
dotnet user-secrets set "EmailSettings:SenderEmail" "tu-email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "tu-app-password"
```

> Para el envío de emails se requiere una [contraseña de aplicación de Google](https://support.google.com/accounts/answer/185833).

### 4. Ejecutar la aplicación

```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:7057`.
Las migraciones de base de datos se aplican automáticamente al iniciar.

### 5. Credenciales iniciales

| Usuario | Contraseña |
|---|---|
| `admin` | `admin` |

---

## Ejecución con Docker

Para ejecutar el contenedor localmente:

```bash
docker build -t tpi-gestion-hogar:latest .

docker run -d \
  --name tpi-hogar \
  -p 8082:8080 \
  -v /ruta/local/uploads:/app/wwwroot/uploads \
  -e DB_CONNECTION="Server=...;Database=HogarDB;..." \
  -e EmailSettings__SenderEmail="tu-email@gmail.com" \
  -e EmailSettings__Password="tu-app-password" \
  tpi-gestion-hogar:latest
```

### Despliegue a producción

El despliegue se realiza automáticamente al hacer push a `main` mediante GitHub Actions. El pipeline construye la imagen, la publica en GitHub Container Registry y la despliega en el VPS vía SSH. Las credenciales se gestionan como GitHub Secrets y nunca se exponen en el repositorio.

---

## Estructura del proyecto

```
TPI-GESTION-HOGAR/
├── Controllers/        # Controladores MVC
├── Models/             # Modelos de dominio
├── Views/              # Vistas Razor
├── Services/           # Lógica de negocio y servicios (email, etc.)
├── DTOs/               # Objetos de transferencia de datos
├── Datos/              # Contexto de base de datos (EF Core)
├── Migrations/         # Migraciones de EF Core
├── wwwroot/            # Archivos estáticos (CSS, JS, imágenes)
├── Properties/         # Configuración de lanzamiento
├── Dockerfile
├── Program.cs
└── appsettings.json
```

---

## Licencia

Este proyecto fue desarrollado con fines académicos en el marco de la Tecnicatura Universitaria en Programación de la UTN FRGP.
