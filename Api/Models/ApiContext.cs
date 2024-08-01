using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class ApiContext : DbContext{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options){}

    public DbSet<UserItem> UserItems {get;set;} = null!;
}