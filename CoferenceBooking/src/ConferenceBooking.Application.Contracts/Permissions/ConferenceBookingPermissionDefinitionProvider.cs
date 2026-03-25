using ConferenceBooking.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ConferenceBooking.Permissions;

public class ConferenceBookingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ConferenceBookingPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ConferenceBookingPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ConferenceBookingResource>(name);
    }
}
