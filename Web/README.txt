App

Un pequeño proyecto para practicar la creacion de web sencilla que consume la Api creada anteriormente con ASP.NET CORE Y C#


COMANDOS USADO EN EL PROYECTO

###############################
CREAR PROYECTO .NET ###########
dotnet new web -o <ProyectName>
###############################

#######################################################################
AGREGAR PAQUETES NuGet ################################################
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
#######################################################################

###################################
AGREGAR PAQUETE SWAGGER ###########
dotnet add package NSwag.AspNetCore
###################################

#######################################################################
CREANDO SCAFFOLDING ###################################################
dotnet aspnet-codegenerator controller -name <ControllerName> -async
-api -m <ModelName> -dc <ContextName[ApiContext]> -outDir Controllers
#####################################################################