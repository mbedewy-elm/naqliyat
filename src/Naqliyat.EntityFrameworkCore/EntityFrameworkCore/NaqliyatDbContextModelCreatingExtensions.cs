using Microsoft.EntityFrameworkCore;
using Naqliyat.Attachments;
using Naqliyat.Notifications;
using Naqliyat.Trips;
using Naqliyat.Trucks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Naqliyat.EntityFrameworkCore;

public static class NaqliyatDbContextModelCreatingExtensions
{
    public static void ConfigureNaqliyat(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        // Trucks
        builder.Entity<Truck>(b =>
        {
            b.ConfigureByConvention();

            // Avoid multiple cascade paths: do not cascade delete from Owner to Truck
            b.HasOne(t => t.Owner)
             .WithMany()
             .HasForeignKey(t => t.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Driver>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<Owner>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<OwnerTruck>(b =>
        {
            b.ConfigureByConvention();

            // Avoid multiple cascade paths involving Truck and OwnerTruck
            b.HasOne(ot => ot.Truck)
             .WithMany()
             .HasForeignKey(ot => ot.TruckId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<TruckType>(b =>
        {
            b.ConfigureByConvention();
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<Country>(b =>
        {
            b.ConfigureByConvention();
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<TruckPicture>(b =>
        {
            b.ConfigureByConvention();

            // Avoid multiple cascade paths involving Truck and TruckPicture
            b.HasOne(tp => tp.Truck)
             .WithMany(t => t.TruckPictures)
             .HasForeignKey(tp => tp.TruckId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Trips
        builder.Entity<Trip>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<TripOwner>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<TripPicture>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<TripStatus>(b =>
        {
            b.ConfigureByConvention();
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        builder.Entity<TripRating>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<Bid>(b =>
        {
            b.ConfigureByConvention();
        });

        builder.Entity<Payment>(b =>
        {
            b.ConfigureByConvention();
        });

        // Attachments
        builder.Entity<Attachment>(b =>
        {
            b.ConfigureByConvention();
        });

        // Notifications
        builder.Entity<Notification>(b =>
        {
            b.ConfigureByConvention();
        });
    }
}
