
using Microsoft.EntityFrameworkCore;
using working_good.business.core.Models.Company;
using working_good.business.infrastructure.DAL.ModelsConfiguration;

namespace working_good.business.infrastructure.DAL;

internal sealed class WgDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<User> Users { get; set; }
    
    public WgDbContext(DbContextOptions<WgDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyModelConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeModelConfiguration());
        modelBuilder.ApplyConfiguration(new UserModelConfiguration());
        modelBuilder.HasDefaultSchema("wg");
    }
}