using Volo.Abp.Modularity;

namespace ConferenceBooking;

/* Inherit from this class for your domain layer tests. */
public abstract class ConferenceBookingDomainTestBase<TStartupModule> : ConferenceBookingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
