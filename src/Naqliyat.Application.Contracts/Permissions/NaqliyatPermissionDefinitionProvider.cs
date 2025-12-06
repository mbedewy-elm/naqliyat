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
        trips.AddChild(NaqliyatPermissions.Trips.Create,      L("Permission:Trips.Create"));
        trips.AddChild(NaqliyatPermissions.Trips.ViewOpen,    L("Permission:Trips.ViewOpen"));
        trips.AddChild(NaqliyatPermissions.Trips.ViewDetails, L("Permission:Trips.ViewDetails"));
        trips.AddChild(NaqliyatPermissions.Trips.Bid,         L("Permission:Trips.Bid"));
        trips.AddChild(NaqliyatPermissions.Trips.ViewBids,    L("Permission:Trips.ViewBids"));
        trips.AddChild(NaqliyatPermissions.Trips.ManageBids,  L("Permission:Trips.ManageBids"));
        trips.AddChild(NaqliyatPermissions.Trips.CreatePayment, L("Permission:Trips.CreatePayment"));
        trips.AddChild(NaqliyatPermissions.Trips.ConfirmArrival, L("Permission:Trips.ConfirmArrival"));
        trips.AddChild(NaqliyatPermissions.Trips.Rate, L("Permission:Trips.Rate"));
        trips.AddChild(NaqliyatPermissions.Trips.DriverTrips, L("Permission:Trips.DriverTrips"));

        var trucks = myGroup.AddPermission(NaqliyatPermissions.Trucks.Default, L("Permission:Trucks"));
        trucks.AddChild(NaqliyatPermissions.Trucks.Create, L("Permission:Trucks.Create"));
        trucks.AddChild(NaqliyatPermissions.Trucks.List, L("Permission:Trucks.List"));

        var drivers = myGroup.AddPermission(NaqliyatPermissions.Drivers.Default, L("Permission:Drivers"));
        drivers.AddChild(NaqliyatPermissions.Drivers.Create, L("Permission:Drivers.Create"));
        drivers.AddChild(NaqliyatPermissions.Drivers.List, L("Permission:Drivers.List"));
        drivers.AddChild(NaqliyatPermissions.Drivers.ViewDetails, L("Permission:Drivers.ViewDetails"));

        var users = myGroup.AddPermission(NaqliyatPermissions.Users.Default, L("Permission:Users"));
        users.AddChild(NaqliyatPermissions.Users.RegisterAsRequester, L("Permission:Users.RegisterAsRequester"));
        users.AddChild(NaqliyatPermissions.Users.RegisterAsOwner, L("Permission:Users.RegisterAsOwner"));
        users.AddChild(NaqliyatPermissions.Users.RegisterAsDriver, L("Permission:Users.RegisterAsDriver"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NaqliyatResource>(name);
    }
}
