using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataLayer;

public class BlablacarContext: IdentityDbContext<User,ApplicationRole,Guid>
{
    private readonly IConfiguration _configuration;
    public BlablacarContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public BlablacarContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(_configuration
            .GetConnectionString("Connection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Trip).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Route> Routes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Trip> Trips { get; set; }
}