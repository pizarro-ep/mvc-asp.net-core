Api

Un pequeño proyecto para practicar la creacion de apis web con ASP.NET CORE Y C#

Prueba de api en: https://[domain]/swagger/index.html


COMANDOS USADO EN EL PROYECTO

####################################################
CREAR PROYECTO .NET ################################
dotnet new webapi --use-controllers -o <ProyectName>
####################################################

###################################################################
AGREGAR PAQUETES NuGet ############################################
dotnet add package Microsoft.EntityFrameworkCore.InMemory
AGREGAR Nuget PARA SCAFFOLDING ####################################
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool uninstall -g dotnet-aspnet-codegenerator
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
###################################################################

#####################################################################
CREANDO SCAFFOLDING #################################################
dotnet aspnet-codegenerator controller -name <ControllerName> -async
-api -m <ModelName> -dc <ContextName[ApiContext]> -outDir Controllers
#####################################################################