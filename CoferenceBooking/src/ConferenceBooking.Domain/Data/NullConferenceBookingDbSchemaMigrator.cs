using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ConferenceBooking.Data;

/* This is used if database provider does't define
 * IConferenceBookingDbSchemaMigrator implementation.
 */
public class NullConferenceBookingDbSchemaMigrator : IConferenceBookingDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
