using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (! context.Movie.Any())
                {
                    context.Movie.AddRange(
                        new Movie
                        {
                            Title = "When Harry Met Sally",
                            ReleaseDate = DateTime.Parse("1989-1-11"),
                            Genre = "Romantic Comedy",
                            Price = 7.99M,
                            Rating = "C"
                        },

                        new Movie
                        {
                            Title = "Ghostbusters ",
                            ReleaseDate = DateTime.Parse("1984-3-13"),
                            Genre = "Comedy",
                            Price = 8.99M,
                            Rating = "A"
                        },

                        new Movie
                        {
                            Title = "Ghostbusters 2",
                            ReleaseDate = DateTime.Parse("1986-2-23"),
                            Genre = "Comedy",
                            Price = 9.99M,
                            Rating = "B"
                        },

                        new Movie
                        {
                            Title = "Rio Bravo",
                            ReleaseDate = DateTime.Parse("1959-4-15"),
                            Genre = "Western",
                            Price = 3.99M,
                            Rating = "D"
                        },

                        new Movie
                        {
                            Title = "Mask",
                            ReleaseDate = DateTime.Parse("1995-4-15"),
                            Genre = "Comedy",
                            Price = 2.99M,
                            Rating = "D"
                        }
                    );
                }

                // Look for any actors.
                if (! context.Actor.Any())
                {
                    context.Actor.AddRange(
                        new Actor
                        {
                            FirstName = "Keanu",
                            LastName = "Reeves",
                            BornDate = new DateTime(1964, 9, 2),
                            Age = DateTime.Now.Year - 1964,
                            ActInMovies = context.Movie.Where(s => s.Title.Contains("Bravo")).ToList()
                        },

                        new Actor
                        {
                            FirstName = "Laurence",
                            LastName = "Fishburne",
                            BornDate = new DateTime(1961, 6, 30),
                            Age = DateTime.Now.Year - 1961,
                            ActInMovies = context.Movie.Where(s => s.Title.Contains("Mask")).ToList()
                        }
                     );
                }

                //Finish Seed Saving Data
                context.SaveChanges();
            }
        }
    }
}