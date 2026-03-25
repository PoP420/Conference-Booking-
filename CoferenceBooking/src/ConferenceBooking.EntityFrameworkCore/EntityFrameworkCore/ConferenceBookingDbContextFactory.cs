using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ConferenceBooking.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ConferenceBookingDbContextFactory : IDesignTimeDbContextFactory<ConferenceBookingDbContext>
{
    public ConferenceBookingDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ConferenceBookingEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ConferenceBookingDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new ConferenceBookingDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ConferenceBooking.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
