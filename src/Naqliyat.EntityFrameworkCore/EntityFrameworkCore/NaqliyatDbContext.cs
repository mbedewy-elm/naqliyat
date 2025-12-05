using Microsoft.EntityFrameworkCore;
using Naqliyat.Attachments;
using Naqliyat.Notifications;
using Naqliyat.Trips;
using Naqliyat.Trucks;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Naqliyat.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class NaqliyatDbContext :
    AbpDbContext<NaqliyatDbContext>,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion

    #region Entities

    public DbSet<Truck> Trucks { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<OwnerTruck> OwnerTrucks { get; set; }
    public DbSet<TruckType> TruckTypes { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<TruckPicture> TruckPictures { get; set; }

    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripOwner> TripOwners { get; set; }
    public DbSet<TripPicture> TripPictures { get; set; }
    public DbSet<TripStatus> TripStatuses { get; set; }
    public DbSet<TripRating> TripRatings { get; set; }
    public DbSet<Bid> Bids { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    #endregion

    public NaqliyatDbContext(DbContextOptions<NaqliyatDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();
        
        builder.ConfigureNaqliyat();
        
        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(NaqliyatConsts.DbTablePrefix + "YourEntities", NaqliyatConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
