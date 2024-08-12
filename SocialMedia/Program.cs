using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the database (SQLite in this case) to use in-memory for testing purposes.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services to the container.
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    // Configure password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    /*options.Password.RequireUniqueChars = 14;*/
    // Here you can configure other options
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Configure the application's cookie policy.
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options => { 
    options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Editar Rol"));
    options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Crear Rol"));
    options.AddPolicy("DeleteRolePolicy",  policy => policy.RequireClaim("Eliminar Rol"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
}
else{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
