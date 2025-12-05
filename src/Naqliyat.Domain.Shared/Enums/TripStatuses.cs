using System;

namespace Naqliyat.Enums
{
    public enum TripStatuses : byte
    {
        OpenForBidding = 1,
        WaitingBidAgreement = 2,
        WaitingPayment = 3,
        OnItsWay = 4,
        Arrived = 5
    }
}
