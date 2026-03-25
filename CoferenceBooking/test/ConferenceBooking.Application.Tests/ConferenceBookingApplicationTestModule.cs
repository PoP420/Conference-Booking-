using Volo.Abp.Modularity;

namespace ConferenceBooking;

[DependsOn(
    typeof(ConferenceBookingApplicationModule),
    typeof(ConferenceBookingDomainTestModule)
)]
public class ConferenceBookingApplicationTestModule : AbpModule
{

}
