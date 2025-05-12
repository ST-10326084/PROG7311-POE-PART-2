using Microsoft.EntityFrameworkCore;
using Core.Models;
using System.Collections.Generic;

namespace Core.Data;
/*
 *  @Reference: dotnet-bot (2025). DbContext Class (System.Data.Entity). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.data.entity.dbcontext?view=entity-framework-6.2.0 [Accessed 12 May 2025].‌
 */
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Farmer> Farmers => Set<Farmer>();
    public DbSet<Product> Products => Set<Product>();
}
