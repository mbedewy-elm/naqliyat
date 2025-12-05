using Naqliyat.Attachments;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class Driver : AuditedEntity<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Identification { get; set; }
        public string LicenseNumber { get; set; }
        public Guid PhotoId { get; set; }
        public int NationalityId { get; set; }
        public Guid UserId { get; set; }

        public virtual Country Nationality { get; set; }
        public virtual Attachment Photo { get; set; }
        public virtual Truck Truck { get; set; }

        public Driver()
        {
        }

        public Driver(
            Guid id,
            string name,
            string phone,
            string identification,
            string licenseNumber,
            int nationalityId,
            Guid userId)
            : base(id)
        {
            Name = name;
            Phone = phone;
            Identification = identification;
            LicenseNumber = licenseNumber;
            NationalityId = nationalityId;
            UserId = userId;
        }

        public Driver(
            Guid id,
            string name,
            string phone,
            string identification,
            string licenseNumber,
            int nationalityId,
            Guid userId,
            Guid photoId)
            : this(id, name, phone, identification, licenseNumber, nationalityId, userId)
        {
            PhotoId = photoId;
        }

        public void SetPhoto(Guid photoId)
        {
            PhotoId = photoId;
        }
    }
}
