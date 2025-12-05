using System;
using Volo.Abp.Domain.Entities.Auditing;
using Naqliyat.Attachments;

namespace Naqliyat.Trucks
{
    public class TruckPicture : AuditedEntity<Guid>
    {
        public Guid TruckId { get; set; }
        public Guid PictureId { get; set; }

        public virtual Truck Truck { get; set; }
        public virtual Attachment Picture { get; set; }

        public TruckPicture()
        {
        }

        public TruckPicture(Guid id, Guid truckId, Guid pictureId)
            : base(id)
        {
            TruckId = truckId;
            PictureId = pictureId;
        }
    }
}
