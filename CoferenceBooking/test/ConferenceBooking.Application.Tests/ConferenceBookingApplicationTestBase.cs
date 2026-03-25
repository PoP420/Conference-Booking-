using Volo.Abp.Modularity;

namespace ConferenceBooking;

public abstract class ConferenceBookingApplicationTestBase<TStartupModule> : ConferenceBookingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
