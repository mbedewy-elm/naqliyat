using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Naqliyat.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class NaqliyatDbContextFactory : IDesignTimeDbContextFactory<NaqliyatDbContext>
{
    public NaqliyatDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        NaqliyatEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<NaqliyatDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new NaqliyatDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Naqliyat.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
