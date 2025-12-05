using Volo.Abp.Domain.Entities;

namespace Naqliyat.Trucks
{
    public class Country : Entity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public Country()
        {

        }

        public Country(string nameAr, string nameEn) : this()
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }

        public Country(int id, string nameAr, string nameEn) : this(nameAr, nameEn)
        {
            Id = id;
        }
    }
}
