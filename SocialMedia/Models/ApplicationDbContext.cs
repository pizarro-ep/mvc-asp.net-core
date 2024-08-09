using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SocialMedia.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder){
            // Configuración de relaciones entre tablas
            base.OnModelCreating(builder); 
            // Relación entre ApplicationUser y ApplicationRole
            builder.Entity<IdentityUserRole<string>>()
                   .HasOne<ApplicationRole>()
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.NoAction); // OnDelete(DeleteBehavior.Restrict); // OnDelete(DeleteBehavior.Cascade);
        }
    }
}