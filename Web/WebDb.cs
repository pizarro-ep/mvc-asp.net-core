using Microsoft.EntityFrameworkCore;

class WebDb : DbContext{
	public WebDb(DbContextOptions<WebDb> options) : base(options) {}

	public DbSet<User> Users => Set<User>();
}
