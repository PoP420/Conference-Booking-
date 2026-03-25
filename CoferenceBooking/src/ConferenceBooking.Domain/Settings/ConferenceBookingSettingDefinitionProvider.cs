using Volo.Abp.Settings;

namespace ConferenceBooking.Settings;

public class ConferenceBookingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ConferenceBookingSettings.MySetting1));
    }
}
