using Xunit;

namespace ConferenceBooking.EntityFrameworkCore;

[CollectionDefinition(ConferenceBookingTestConsts.CollectionDefinitionName)]
public class ConferenceBookingEntityFrameworkCoreCollection : ICollectionFixture<ConferenceBookingEntityFrameworkCoreFixture>
{

}
