using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebDb>(opt => opt.UseInMemoryDatabase("WebList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "WebAPI";
    config.Title = "WebAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "WebAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var userItems = app.MapGroup("/useritems");

userItems.MapGet("/",GetAllUsers);
userItems.MapGet("/active", GetActiveUsers);
userItems.MapGet("/{id}", GetUser);
userItems.MapPost("/", CreateUser);
userItems.MapPut("/{id}", UpdateUser);
userItems.MapDelete("/{id}", DeleteUser);


app.Run();

// #########################################################################################
static async Task<IResult> GetAllUsers(WebDb db){
    //return TypedResults.Ok(await db.Users.ToListAsync());
    return TypedResults.Ok(await db.Users.Select(x => new UserItemDTO(x)).ToArrayAsync());
}
static async Task<IResult> GetActiveUsers(WebDb db){
    // return TypedResults.Ok(await db.Users.Where(t => t.IsActive).ToListAsync());
    return TypedResults.Ok(await db.Users.Where(t => t.IsActive).Select(x => new UserItemDTO(x)).ToListAsync());
}
static async Task<IResult> GetUser(int id, WebDb db){
    // return await db.Users.FindAsync(id) is User user ? TypedResults.Ok(user) : TypedResults.NotFound();
    return await db.Users.FindAsync(id) is User user ? TypedResults.Ok(new UserItemDTO(user)) : TypedResults.NotFound();
}
//static async Task<IResult> CreateUser(User user, WebDb db){
static async Task<IResult> CreateUser(UserItemDTO userItemDTO, WebDb db){
    var userItem = new User {//
        IsActive = userItemDTO.IsActive, //
        Username = userItemDTO.Username //
    }; //
    //db.Users.Add(user);
    db.Users.Add(userItem);
    await db.SaveChangesAsync();
    userItemDTO = new UserItemDTO(userItem); //
    //return TypedResults.Created($"/useritems/{user.Id}", user);
    return TypedResults.Created($"/useritems/{userItem.Id}", userItemDTO);
}
//static async Task<IResult> UpdateUser(int id, User inputUser, WebDb db){
static async Task<IResult> UpdateUser(int id, UserItemDTO userItemDTO, WebDb db){
    var user = await db.Users.FindAsync(id);
    if (user is null) return TypedResults.NotFound();

    // user.Username = inputUser.Username; 
    // user.IsActive = inputUser.IsActive;
    user.Username = userItemDTO.Username;
    user.IsActive = userItemDTO.IsActive;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}
static async Task<IResult> DeleteUser(int id, WebDb db){
    if (await db.Users.FindAsync(id) is User user){
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}
