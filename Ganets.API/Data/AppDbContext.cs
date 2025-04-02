namespace Ganets.API.Data;

using Ganets.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Свойства DbSet для доступа к таблицам базы данных
    public DbSet<Category> Categories { get; set; }
    public DbSet<Gadget> Gadgets { get; set; }
}


