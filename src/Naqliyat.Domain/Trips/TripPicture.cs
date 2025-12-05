using Naqliyat.Attachments;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class TripPicture : AuditedEntity<Guid>
    {
        public Guid TripId { get; set; }
        public Guid PictureId { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual Attachment Picture { get; set; }

        public TripPicture()
        {
        }

        public TripPicture(Guid id, Guid tripId, Guid pictureId)
            : base(id)
        {
            TripId = tripId;
            PictureId = pictureId;
        }
    }
}
