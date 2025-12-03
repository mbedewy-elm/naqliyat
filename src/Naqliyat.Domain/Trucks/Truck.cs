using Naqliyat.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class Truck : FullAuditedAggregateRoot<Guid>
    {
        public string LicenseNumber { get; set; }
        public TruckTypes TruckType { get; set; }
        public TemperatureRequirements TemperatureRequirement { get; set; }
        public Guid OwnerId { get; set; }
        public Guid DriverId { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Driver Driver { get; set; }

        public Truck()
        {
            
        }
    }
}
