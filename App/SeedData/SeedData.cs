using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace App.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any PersonModels.
            if (context.PersonModel.Any())
            {
                return;   // DB has been seeded
            }
            context.PersonModel.AddRange(
                new PersonModel
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Rating = "R"
                },
                new PersonModel
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Rating = "S"
                },
                new PersonModel
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Rating = "R"
                },
                new PersonModel
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Rating = "S"
                }
            );
            context.SaveChanges();

            if (context.User.Any()){return;}

            context.User.AddRange(
                new User {
                    Username = "User 1",
                    Email = "user@gmail.com",
                    Password = "12345678"
                },
                new User {
                    Username = "User 2",
                    Email = "user_2@gmail.com",
                    Password = "87654321"
                }
            );
            context.SaveChanges();
        }
    }
}