using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configuration;

public class TripConfiguration: IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(trip => trip.Id);
        
        builder.HasOne(trip => trip.Route)
            .WithMany(route => route.Trips)
            .HasForeignKey(trip=>trip.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(trip => trip.DepartureAt)
            .IsRequired();
    }
}