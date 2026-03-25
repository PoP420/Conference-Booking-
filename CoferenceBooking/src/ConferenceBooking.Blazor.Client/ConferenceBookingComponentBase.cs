using ConferenceBooking.Localization;
using Volo.Abp.AspNetCore.Components;

namespace ConferenceBooking.Blazor.Client;

public abstract class ConferenceBookingComponentBase : AbpComponentBase
{
    protected ConferenceBookingComponentBase()
    {
        LocalizationResource = typeof(ConferenceBookingResource);
    }
}
