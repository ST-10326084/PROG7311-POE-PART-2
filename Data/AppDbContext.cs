using Microsoft.EntityFrameworkCore;
using Core.Models;
using System.Collections.Generic;

namespace Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Farmer> Farmers => Set<Farmer>();
    public DbSet<Product> Products => Set<Product>();
}
