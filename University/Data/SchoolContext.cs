using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data {
    public class SchoolContext : DbContext {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) {}

        public DbSet<Course> Courses {get;set;}
        public DbSet<Enrollment> Enrollments {get;set;}
        public DbSet<Student> Students {get;set;}

        // Cambiar el nombre por defecto con le que se crean las tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
    }
}