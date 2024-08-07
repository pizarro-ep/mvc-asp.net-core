Gestión de libros

Un pequeño proyecto para practicar la creacion de páginas web con ASP.NET CORE Y C#, se realiza el enfoque MVC(Modelo-Vista-Controlador), se usa una base de datos SQLite para probar las acciones de Creación, Lectura, Actualizacion y Eliminacion de datos.


COMANDOS USADO EN EL PROYECTO

CREAR PROYECTO .NET
- dotnet new mvc -o <ProyectName>

AGREGAR PAQUETES NuGet
- dotnet tool uninstall --global dotnet-aspnet-codegenerator
- dotnet tool install --global dotnet-aspnet-codegenerator
- dotnet tool uninstall --global dotnet-ef
- dotnet tool install --global dotnet-ef
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.SQLite
- dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
PAQUETES DE AUTENTICACION
- dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
- dotnet add package Microsoft.AspNetCore.Identity.UI
- dotnet add package Microsoft.AspNetCore.Authorization

CREANDO SCAFFOLDING
- dotnet aspnet-codegenerator controller -name <ControllerName> -m <ModelName> -dc <AppDbContext> --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
- dotnet aspnet-codegenerator identity --useDefaultUI -dc <AppDbContext> <ProjectName>Auth --userClass <ProjectName>User

CREAR MIGRACIÓN
- dotnet ef migrations add InitialCreate
- dotnet ef database update

OTROS COMANDOS
- dotnet run
- dotnet build