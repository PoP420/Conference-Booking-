using ConferenceBooking.Samples;
using Xunit;

namespace ConferenceBooking.EntityFrameworkCore.Applications;

[Collection(ConferenceBookingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ConferenceBookingEntityFrameworkCoreTestModule>
{

}
