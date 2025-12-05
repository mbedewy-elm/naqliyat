using System.Collections.Generic;
using System.Threading.Tasks;
using Naqliyat.Enums;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Naqliyat.Trucks
{
    public class TruckTypeDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<TruckType, TruckTypes> _truckTypeRepository;

        public TruckTypeDataSeedContributor(IRepository<TruckType, TruckTypes> truckTypeRepository)
        {
            _truckTypeRepository = truckTypeRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (await _truckTypeRepository.GetCountAsync() > 0)
            {
                return;
            }

            var truckTypes = GetTruckTypes();

            foreach (var type in truckTypes)
            {
                await _truckTypeRepository.InsertAsync(type, autoSave: true);
            }
        }

        private static List<TruckType> GetTruckTypes()
        {
            return new List<TruckType>
            {
                // We skip Unknown = 0
                new TruckType(TruckTypes.SemiTruck,         "Semi Truck",                  "رأس شاحنة"),
                new TruckType(TruckTypes.DayCab,             "Day Cab",                    "رأس شاحنة نهاري"),
                new TruckType(TruckTypes.SleeperCab,         "Sleeper Cab",                "رأس شاحنة بسرير"),
                new TruckType(TruckTypes.DryVan,             "Dry Van",                    "مقطورة جافة"),
                new TruckType(TruckTypes.BoxTruck,           "Box Truck",                  "شاحنة صندوقية"),
                new TruckType(TruckTypes.CurtainSide,        "Curtain Side",               "مقطورة بستائر جانبية"),
                new TruckType(TruckTypes.Reefer,             "Reefer",                     "مبردة قياسية"),
                new TruckType(TruckTypes.RefrigeratedChilled,"Refrigerated (Chilled)",     "مبردة (مبرد)"),
                new TruckType(TruckTypes.RefrigeratedFrozen, "Refrigerated (Frozen)",      "مبردة (مجمد)"),
                new TruckType(TruckTypes.RefrigeratedDeepFrozen, "Refrigerated (Deep Frozen)", "مبردة (تجميد عميق)"),
                new TruckType(TruckTypes.Insulated,          "Insulated",                  "معزولة"),
                new TruckType(TruckTypes.Flatbed,            "Flatbed",                    "سطحة"),
                new TruckType(TruckTypes.StepDeck,           "Step Deck",                  "سطحة متعددة المستويات"),
                new TruckType(TruckTypes.DoubleDrop,         "Double Drop",                "سطحة منخفضة جداً"),
                new TruckType(TruckTypes.Lowboy,             "Lowboy",                     "سطحة منخفضة"),
                new TruckType(TruckTypes.Conestoga,          "Conestoga",                  "كونيستوغا"),
                new TruckType(TruckTypes.Hotshot,            "Hotshot",                    "هاتشوت"),
                new TruckType(TruckTypes.Tanker,             "Tanker",                     "ناقلة"),
                new TruckType(TruckTypes.PneumaticTanker,    "Pneumatic Tanker",           "ناقلة هوائية"),
                new TruckType(TruckTypes.CarCarrier,         "Car Carrier",                "ناقلة سيارات"),
                new TruckType(TruckTypes.LoggingTruck,       "Logging Truck",              "شاحنة نقل أخشاب"),
                new TruckType(TruckTypes.DumpTruck,          "Dump Truck",                 "قلاب"),
                new TruckType(TruckTypes.GarbageTruck,       "Garbage Truck",              "شاحنة نفايات"),
                new TruckType(TruckTypes.CementMixer,        "Cement Mixer",               "خلاطة خرسانة"),
                new TruckType(TruckTypes.HeavyHaul,          "Heavy Haul",                 "نقل ثقيل"),
                new TruckType(TruckTypes.OversizeLoad,       "Oversize Load",              "حمولة كبيرة الحجم"),
                new TruckType(TruckTypes.SprinterVan,        "Sprinter Van",               "فان سبرنتر"),
                new TruckType(TruckTypes.CargoVan,           "Cargo Van",                  "فان بضائع"),
                new TruckType(TruckTypes.PickUp,             "Pickup",                     "بيك أب")
            };
        }
    }
}
