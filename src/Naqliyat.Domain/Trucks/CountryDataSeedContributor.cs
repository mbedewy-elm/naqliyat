using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Naqliyat.Trucks
{
    public class CountryDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Country, int> _countryRepository;

        public CountryDataSeedContributor(IRepository<Country, int> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (await _countryRepository.GetCountAsync() > 0)
            {
                return;
            }

            var countries = GetArabicCountries();

            foreach (var country in countries)
            {
                await _countryRepository.InsertAsync(country, autoSave: true);
            }
        }

        private static List<Country> GetArabicCountries()
        {
            return new List<Country>
            {
                new Country( 1, "السعودية", "Saudi Arabia"),
                new Country( 2, "الإمارات", "United Arab Emirates"),
                new Country( 3, "الكويت", "Kuwait"),
                new Country( 4, "قطر", "Qatar"),
                new Country( 5, "البحرين", "Bahrain"),
                new Country( 6, "عمان", "Oman"),
                new Country( 7, "اليمن", "Yemen"),
                new Country( 8, "مصر", "Egypt"),
                new Country( 9, "الأردن", "Jordan"),
                new Country(10, "لبنان", "Lebanon"),
                new Country(11, "سوريا", "Syria"),
                new Country(12, "العراق", "Iraq"),
                new Country(13, "فلسطين", "Palestine"),
                new Country(14, "ليبيا", "Libya"),
                new Country(15, "تونس", "Tunisia"),
                new Country(16, "الجزائر", "Algeria"),
                new Country(17, "المغرب", "Morocco"),
                new Country(18, "السودان", "Sudan"),
                new Country(19, "موريتانيا", "Mauritania"),
                new Country(20, "جيبوتي", "Djibouti"),
                new Country(21, "الصومال", "Somalia"),
                new Country(22, "جزر القمر", "Comoros")
            };
        }
    }
}
