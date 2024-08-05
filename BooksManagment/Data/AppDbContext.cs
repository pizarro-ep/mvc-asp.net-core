using BooksManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagment.Data{
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {} 

        public DbSet<Book> Books {get; set;}
        public DbSet<Genre> Genres {get; set;}
        public DbSet<Author> Authors {get; set;}
        public DbSet<Review> Reviews {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            // Change default table name
            //modelBuilder.Entity<Book>().ToTable("Books");

            base.OnModelCreating(modelBuilder);

            // Configuración de la relaciones
            modelBuilder.Entity<Book>().HasOne(b => b.Genre).WithMany(g => g.Books).HasForeignKey(b => b.GenreID);
            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorID);
            modelBuilder.Entity<Review>().HasOne(r => r.Book).WithMany(b => b.Reviews).HasForeignKey(r => r.BookID);
            modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserID);
            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleID);
        }
    }
}