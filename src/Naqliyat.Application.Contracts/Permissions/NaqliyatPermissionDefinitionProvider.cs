using Naqliyat.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Naqliyat.Permissions;

public class NaqliyatPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NaqliyatPermissions.GroupName);

        var trips = myGroup.AddPermission(NaqliyatPermissions.Trips.Default, L("Permission:Trips"));
        trips.AddChild(NaqliyatPermissions.Trips.Create, L("Permission:Trips.Create"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NaqliyatResource>(name);
    }
}
