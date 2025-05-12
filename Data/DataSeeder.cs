// @reference: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
using Core.Data;
using Core.Models;
using Core.Services;

namespace Core
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "farmer1", PasswordHash = AuthService.HashPasswordStatic("pass123"), Role = "Farmer" },
                    new User { Username = "employee1", PasswordHash = AuthService.HashPasswordStatic("pass123"), Role = "Employee" }
            );

            }

            if (!context.Farmers.Any())
            {
                var farmer = new Farmer { Name = "farmer1", Location = "Farmville" };
                context.Farmers.Add(farmer);
                context.SaveChanges();

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Tomatoes",
                        Category = "Vegetable",
                        ProductionDate = DateTime.UtcNow.AddDays(-7),
                        FarmerId = farmer.Id
                    },
                    new Product
                    {
                        Name = "Apples",
                        Category = "Fruit",
                        ProductionDate = DateTime.UtcNow.AddDays(-10),
                        FarmerId = farmer.Id
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
