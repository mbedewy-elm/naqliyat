using System;
using Volo.Abp.Domain.Entities.Auditing;
using Naqliyat.Trucks;

namespace Naqliyat.Trips
{
    public class TripOwner : AuditedEntity<Guid>
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }

        public virtual Trip Trip { get; set; }

        public TripOwner()
        {
        }

        public TripOwner(
            Guid id,
            Guid tripId,
            bool isPrimary)
            : base(id)
        {
            TripId = tripId;
        }
    }
}
