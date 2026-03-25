using ConferenceBooking.Samples;
using Xunit;

namespace ConferenceBooking.EntityFrameworkCore.Domains;

[Collection(ConferenceBookingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ConferenceBookingEntityFrameworkCoreTestModule>
{

}
