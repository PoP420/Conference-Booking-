using Microsoft.Extensions.Localization;
using ConferenceBooking.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ConferenceBooking;

[Dependency(ReplaceServices = true)]
public class ConferenceBookingBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ConferenceBookingResource> _localizer;

    public ConferenceBookingBrandingProvider(IStringLocalizer<ConferenceBookingResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
