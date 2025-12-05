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
    }
}
