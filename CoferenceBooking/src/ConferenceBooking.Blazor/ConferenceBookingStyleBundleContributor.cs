using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace ConferenceBooking.Blazor;

public class ConferenceBookingStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
