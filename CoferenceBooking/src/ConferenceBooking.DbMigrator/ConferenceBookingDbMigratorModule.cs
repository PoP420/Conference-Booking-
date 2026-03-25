using ConferenceBooking.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ConferenceBooking.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ConferenceBookingEntityFrameworkCoreModule),
    typeof(ConferenceBookingApplicationContractsModule)
)]
public class ConferenceBookingDbMigratorModule : AbpModule
{
}
