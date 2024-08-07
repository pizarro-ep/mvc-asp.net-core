using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BooksManagment.Models;
using BooksManagment.Data;
using BooksManagment.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext para usar SQLite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar el filtro de excepciones de base de datos para entornos de desarrollo
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Registrar UserService
builder.Services.AddScoped<UserService>();

// AUTENTICATE
// Configurar la autenticacion de cookies
builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", config => {
    config.Cookie.Name = "UserLoginCookie"; // Nombre del cookie
    config.LoginPath = "/Account/Login"; // Ruta para el inicio de sesión
});
/*
// Configurar Identity - drop
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// Configurar autenticación y autorización
builder.Services.AddAuthentication();
*/
// configurar la autorización
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AuthenticationUser", policy => policy.RequireAuthenticatedUser());
    //options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    //options.AddPolicy("RequireNormalRole", policy => policy.RequireRole("Normal"));
    //options.AddPolicy("RequireInvitadoRole", policy => policy.RequireRole("Invitado"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

/*/ Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    await SeedData.Initialize(services, userManager);
}*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Configurar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

CreateDbIfNotExists(app);

app.Run();


// Método para inicializar la base de datos si no existe
static void CreateDbIfNotExists(IHost app)
{
    using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            DbInitializer.Initialize(context);
        } catch (Exception ex) {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Un erro ha ocurrido mientras costruia la base de datos.");
        }
    }
}
