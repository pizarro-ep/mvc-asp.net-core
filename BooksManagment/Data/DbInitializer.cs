using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BooksManagment.Models;

namespace BooksManagment.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Verifica si ya existen datos
            if (context.Users.Any()) { return;  }

            var roles = new Role[]{
                new Role { Name = "Administrador" },
                new Role { Name = "Normal" },
                new Role { Name = "Invitado" },
            };
            foreach(Role r in roles){context.Roles.Add(r);}
            context.SaveChanges();

            var users = new User[] {
                new User{ Username = "johndoe", Password = "hash1", Email = "johndoe@example.com", RoleID = 1 },
                new User{ Username = "janesmith", Password = "hash2", Email = "janesmith@example.com", RoleID = 2 },
                new User{ Username = "alicejones", Password = "hash3", Email = "alicejones@example.com", RoleID = 2 },
                new User{ Username = "bobbrown", Password = "hash4", Email = "bobbrown@example.com", RoleID = 2 },
                new User{ Username = "carolwhite", Password = "hash5", Email = "carolwhite@example.com", RoleID = 2 },
            };
            foreach(User u in users) {context.Users.Add(u);}
            context.SaveChanges();

            var authors = new Author[] {
                new Author { Names = "John", Surnames = "Doe", Biography = "John Doe is a seasoned software developer with over 20 years of experience.", BirthDate = DateTime.Parse("1980-01-01"),},
                new Author { Names = "Jane", Surnames = "Smith", Biography = "Jane Smith is a software architect and a prolific writer.", BirthDate = DateTime.Parse("1982-02-02")},
                new Author { Names = "Alice", Surnames = "Jones", Biography = "Alice Jones is an expert in C# and .NET technologies.", BirthDate = DateTime.Parse("1984-03-03")},
                new Author { Names = "Bob", Surnames = "Brown", Biography = "Bob Brown is a web development guru with a passion for teaching.", BirthDate = DateTime.Parse("1986-04-04")},
                new Author { Names = "Carol", Surnames = "White", Biography = "Carol White is a front-end developer specializing in JavaScript frameworks.", BirthDate = DateTime.Parse("1988-05-05")},
                new Author { Names = "Dave", Surnames = "Black", Biography = "Dave Black is an Angular expert and a seasoned developer.", BirthDate = DateTime.Parse("1990-06-06")},
                new Author { Names = "Eve", Surnames = "Green", Biography = "Eve Green is a React and Redux specialist.", BirthDate = DateTime.Parse("1992-07-07")},
                new Author { Names = "Frank", Surnames = "Yellow", Biography = "Frank Yellow is a Vue.js enthusiast and a skilled developer.", BirthDate = DateTime.Parse("1994-08-08")},
                new Author { Names = "Grace", Surnames = "Pink", Biography = "Grace Pink is a database developer with a deep knowledge of SQL.", BirthDate = DateTime.Parse("1996-09-09")},
                new Author { Names = "Henry", Surnames = "Blue", Biography = "Henry Blue is a Python programmer and data scientist.", BirthDate = DateTime.Parse("1998-10-10")}
            };
            foreach(Author a in authors) { context.Authors.Add(a);}
            context.SaveChanges();

            var genres = new Genre[] { 
                new Genre { Name = "Programming" }, 
                new Genre { Name = "Database" }, 
                new Genre { Name = "Web Development" }
            };
            foreach(Genre g in genres) { context.Genres.Add(g);}
            context.SaveChanges();

            var books = new Book[]{
                new Book { Title = "ASP.NET Core Basics",Description = "A comprehensive guide to ASP.NET Core.", PublicationDate = DateTime.Parse("2022-01-01"), GenreID = 1, AuthorID = 1 },
                new Book { Title = "Entity Framework Core", Description = "Mastering Entity Framework Core.", PublicationDate = DateTime.Parse("2022-02-01"), GenreID = 2, AuthorID = 2 },
                new Book { Title = "C# Programming", Description = "A complete guide to C# programming.", PublicationDate = DateTime.Parse("2022-03-01"), GenreID = 1, AuthorID = 3 },
                new Book { Title = "Web Development", Description = "The ultimate guide to web development.", PublicationDate = DateTime.Parse("2022-04-01"), GenreID = 3, AuthorID = 4 },
                new Book { Title = "JavaScript Essentials", Description = "Learn the essentials of JavaScript.", PublicationDate = DateTime.Parse("2022-05-01"), GenreID = 3, AuthorID = 5 },
                new Book { Title = "Angular for Beginners", Description = "An introduction to Angular.", PublicationDate = DateTime.Parse("2022-06-01"), GenreID = 1, AuthorID = 6 },
                new Book { Title = "React and Redux", Description = "Master React and Redux.", PublicationDate = DateTime.Parse("2022-07-01"), GenreID = 2, AuthorID = 7 },
                new Book { Title = "Vue.js Guide", Description = "A comprehensive guide to Vue.js.", PublicationDate = DateTime.Parse("2022-08-01"), GenreID = 3, AuthorID = 8 },
                new Book { Title = "SQL for Developers", Description = "Learn SQL for database development.", PublicationDate = DateTime.Parse("2022-09-01"), GenreID = 1, AuthorID = 9 },
                new Book { Title = "Python Programming", Description = "A complete guide to Python programming.", PublicationDate = DateTime.Parse("2022-10-01"), GenreID = 2, AuthorID = 10 }
            };
            foreach (Book b in books) { context.Books.Add(b);}
            context.SaveChanges();

            var reviews = new Review[]{
                new Review { Rating = 5, Comment = "Excellent book on ASP.NET Core!", ReviewDate = DateTime.Parse("2023-01-01"), BookID = 1, UserID = 2 },
                new Review { Rating = 4, Comment = "Very good book", ReviewDate = DateTime.Parse("2023-01-01"), BookID = 2, UserID = 3 }
            };
            foreach(Review r in reviews) {context.Reviews.Add(r);}
            context.SaveChanges();
        }
    }
}