using DynamicQueryBuilderDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicQueryBuilderDemo.Data;

internal class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("DynamicQueryDb");
}
