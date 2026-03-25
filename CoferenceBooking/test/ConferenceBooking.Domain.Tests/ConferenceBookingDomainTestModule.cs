using Volo.Abp.Modularity;

namespace ConferenceBooking;

[DependsOn(
    typeof(ConferenceBookingDomainModule),
    typeof(ConferenceBookingTestBaseModule)
)]
public class ConferenceBookingDomainTestModule : AbpModule
{

}
