using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConferenceBooking.Data;
using Volo.Abp.DependencyInjection;

namespace ConferenceBooking.EntityFrameworkCore;

public class EntityFrameworkCoreConferenceBookingDbSchemaMigrator
    : IConferenceBookingDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreConferenceBookingDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ConferenceBookingDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ConferenceBookingDbContext>()
            .Database
            .MigrateAsync();
    }
}
