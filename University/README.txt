University

University es una aplicación de muestra que demuestra cómo utilizar Entity Framework Core en un Aplicación web ASP.NET Core MVC.


#######################################################################
AGREGAR PAQUETES NuGet NECESARIOS #####################################
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
#######################################################################

#####################################################################
CREANDO SCAFFOLDING #################################################
dotnet aspnet-codegenerator controller -name <ControllerNatoToCreate> 
-m Student -dc <ContextName> --relativeFolderPath Controllers 
--useDefaultLayout --referenceScriptLibraries
#####################################################################

######################################
CREAR MIGRACIÓN ######################
dotnet ef migrations add InitialCreate
dotnet ef database update
######################################