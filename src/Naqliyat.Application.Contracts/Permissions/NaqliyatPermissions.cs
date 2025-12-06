namespace Naqliyat.Permissions;

public static class NaqliyatPermissions
{
    public const string GroupName = "Naqliyat";

    public static class Trips
    {
        public const string Default = GroupName + ".Trips";
        public const string Create   = Default + ".Create";
        public const string ViewOpen   = Default + ".ViewOpen";
        public const string ViewDetails = Default + ".ViewDetails";
        public const string Bid        = Default + ".Bid";
        public const string ViewBids   = Default + ".ViewBids";
        public const string ManageBids = Default + ".ManageBids";
        public const string CreatePayment = Default + ".CreatePayment";
        public const string ConfirmArrival = Default + ".ConfirmArrival";
        public const string Rate = Default + ".Rate";
        public const string DriverTrips = Default + ".DriverTrips";
    }

    public static class Trucks
    {
        public const string Default = GroupName + ".Trucks";
        public const string Create = Default + ".Create";
        public const string List = Default + ".List";
    }

    public static class Drivers
    {
        public const string Default = GroupName + ".Drivers";
        public const string Create = Default + ".Create";
        public const string List = Default + ".List";
        public const string ViewDetails = Default + ".ViewDetails";
    }

    public static class Users
    {
        public const string Default = GroupName + ".Users";
        public const string RegisterAsRequester = Default + ".RegisterAsRequester";
        public const string RegisterAsOwner = Default + ".RegisterAsOwner";
        public const string RegisterAsDriver = Default + ".RegisterAsDriver";
    }
}
