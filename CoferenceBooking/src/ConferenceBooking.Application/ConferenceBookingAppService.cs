using ConferenceBooking.Localization;
using Volo.Abp.Application.Services;

namespace ConferenceBooking;

/* Inherit your application services from this class.
 */
public abstract class ConferenceBookingAppService : ApplicationService
{
    protected ConferenceBookingAppService()
    {
        LocalizationResource = typeof(ConferenceBookingResource);
    }
}
