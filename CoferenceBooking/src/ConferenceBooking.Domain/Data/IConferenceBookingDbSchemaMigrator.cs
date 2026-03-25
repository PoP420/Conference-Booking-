using System.Threading.Tasks;

namespace ConferenceBooking.Data;

public interface IConferenceBookingDbSchemaMigrator
{
    Task MigrateAsync();
}
