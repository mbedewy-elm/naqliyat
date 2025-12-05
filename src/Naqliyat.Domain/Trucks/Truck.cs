using Naqliyat.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class Truck : FullAuditedAggregateRoot<Guid>
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string LicenseNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public TruckTypes TruckTypeId { get; set; }
        public TemperatureRequirements TemperatureRequirement { get; set; }
        public Guid OwnerId { get; set; }
        public Guid DriverId { get; set; }

        public double MaxLoad { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual TruckType TruckType { get; set; }
        public virtual ICollection<TruckPicture> TruckPictures { get; set; }

        public Truck()
        {
            TruckPictures = new HashSet<TruckPicture>();
        }

        public Truck(
            Guid id,
            string make,
            string model,
            string year,
            string licenseNumber,
            string registrationNumber,
            TruckTypes truckTypeId,
            TemperatureRequirements temperatureRequirement,
            Guid ownerId,
            double maxLoad)
            : base(id)
        {
            Make = make;
            Model = model;
            Year = year;
            LicenseNumber = licenseNumber;
            RegistrationNumber = registrationNumber;
            TruckTypeId = truckTypeId;
            TemperatureRequirement = temperatureRequirement;
            OwnerId = ownerId;
            MaxLoad = maxLoad;
            TruckPictures = new HashSet<TruckPicture>();
        }

        public Truck(
            Guid id,
            string make,
            string model,
            string year,
            string licenseNumber,
            string registrationNumber,
            TruckTypes truckTypeId,
            TemperatureRequirements temperatureRequirement,
            Guid ownerId,
            Guid driverId,
            double maxLoad)
            : this(id, make, model, year, licenseNumber, registrationNumber, truckTypeId, temperatureRequirement, ownerId, maxLoad)
        {
            DriverId = driverId;
        }

        public void SetDriver(Guid driverId)
        {
            DriverId = driverId;
        }
    }
}
