using Naqliyat.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class TripStatus : AuditedEntity<TripStatuses>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public TripStatus()
        {
        }

        public TripStatus(TripStatuses id, string nameEn, string nameAr)
            : base(id)
        {
            NameEn = nameEn;
            NameAr = nameAr;
        }
    }
}
