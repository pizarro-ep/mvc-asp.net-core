App

Un pequeño proyecto para practicar la creacion de páginas web con ASP.NET CORE Y C#, se realiza el enfoque MVC(Modelo-Vista-Controlador), se usa una base de datos SQLite para probar las acciones de Creación, Lectura, Actualizacion y Eliminacion de datos


COMANDOS USADO EN EL PROYECTO

###############################
CREAR PROYECTO .NET ###########
dotnet new mvc -o <ProyectName>
###############################

###################################################################
AGREGAR PAQUETES NuGet ############################################
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
###################################################################

############################################################################
CREANDO SCAFFOLDING ########################################################
dotnet aspnet-codegenerator controller -name <ControllerName> -m <ModelName> 
-dc <AppDbContext> --relativeFolderPath Controllers --useDefaultLayout
 --referenceScriptLibraries --databaseProvider sqlite
############################################################################

######################################
CREAR MIGRACIÓN ######################
dotnet ef migrations add InitialCreate
dotnet ef database update
######################################

######################################
OTROS COMANDOS #######################
dotnet run # iniciar la aplicacion
dotnet build # construir la aplicacion
######################################
