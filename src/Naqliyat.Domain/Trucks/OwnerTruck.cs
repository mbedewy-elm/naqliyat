using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class OwnerTruck : AuditedEntity<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid TruckId { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Truck Truck { get; set; }

        public OwnerTruck()
        {
        }

        public OwnerTruck(Guid id, Guid ownerId, Guid truckId)
            : base(id)
        {
            OwnerId = ownerId;
            TruckId = truckId;
        }
    }
}
