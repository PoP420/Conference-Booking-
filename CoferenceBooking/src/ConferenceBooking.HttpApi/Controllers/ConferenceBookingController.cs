using ConferenceBooking.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ConferenceBooking.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ConferenceBookingController : AbpControllerBase
{
    protected ConferenceBookingController()
    {
        LocalizationResource = typeof(ConferenceBookingResource);
    }
}
