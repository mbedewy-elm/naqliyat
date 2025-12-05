using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class TripRating : AuditedEntity<Guid>
    {
        public Guid TripId { get; set; }
        public int Rate { get; set; }
        public string Note { get; set; }

        public virtual Trip Trip { get; set; }

        public TripRating()
        {
        }

        public TripRating(Guid id, Guid tripId, int rate, string note)
            : base(id)
        {
            TripId = tripId;
            Rate = rate;
            Note = note;
        }
    }
}
