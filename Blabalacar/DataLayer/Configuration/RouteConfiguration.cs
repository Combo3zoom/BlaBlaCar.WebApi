using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configuration;

public class RouteConfiguration: IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    { 
        builder.HasKey(route => route.Id);
        
        builder.Property(route => route.StartRoute)
            .HasMaxLength(50);
        
        builder.Property(route => route.EndRoute)
            .HasMaxLength(50);
    }
}