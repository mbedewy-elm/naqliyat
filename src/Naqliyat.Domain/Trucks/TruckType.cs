using Naqliyat.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class TruckType : AuditedEntity<TruckTypes>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public TruckType()
        {

        }

        public TruckType(TruckTypes id, string nameAr, string nameEn) : this()
        {
            Id = id;
            NameAr = nameAr;
            NameEn = nameEn;
        }
    }
}
