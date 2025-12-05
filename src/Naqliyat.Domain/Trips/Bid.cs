using Naqliyat.Enums;
using Naqliyat.Trucks;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class Bid : AuditedEntity<Guid>
    {
        public Guid TripId { get; set; }
        public Guid TruckId { get; set; }
        public double Price { get; set; }
        public DateTime ArrivalDate { get; set; }
        public BidStatus StatusId { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual Truck Truck { get; set; }

        public Bid()
        {
            StatusId = BidStatus.New;
        }

        public Bid(Guid tripId, Guid truckId, double price, DateTime arrivalDate) : this()
        {
            TripId = tripId;
            TruckId = truckId;
            Price = price;
            ArrivalDate = arrivalDate;
        }

        public Bid Accept()
        {
            StatusId = BidStatus.Accepted;
            return this;
        }

        public Bid Reject()
        {
            StatusId = BidStatus.Rejected;
            return this;
        }
        public Bid NotChoosen()
        {
            StatusId = BidStatus.NotChoosen;
            return this;
        }
    }
}
