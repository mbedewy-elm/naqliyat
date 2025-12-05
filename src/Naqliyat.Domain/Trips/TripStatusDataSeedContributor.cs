using System.Collections.Generic;
using System.Threading.Tasks;
using Naqliyat.Enums;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Naqliyat.Trips
{
    public class TripStatusDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<TripStatus, TripStatuses> _tripStatusRepository;

        public TripStatusDataSeedContributor(IRepository<TripStatus, TripStatuses> tripStatusRepository)
        {
            _tripStatusRepository = tripStatusRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (await _tripStatusRepository.GetCountAsync() > 0)
            {
                return;
            }

            var statuses = GetTripStatuses();

            foreach (var status in statuses)
            {
                await _tripStatusRepository.InsertAsync(status, autoSave: true);
            }
        }

        private static List<TripStatus> GetTripStatuses()
        {
            return new List<TripStatus>
            {
                new TripStatus(TripStatuses.OpenForBidding,       "Open for Bidding",      "مفتوح للمزايدة"),
                new TripStatus(TripStatuses.WaitingBidAgreement, "Waiting Bid Agreement", "في انتظار قبول العرض"),
                new TripStatus(TripStatuses.WaitingPayment,      "Waiting Payment",       "في انتظار الدفع"),
                new TripStatus(TripStatuses.OnItsWay,            "On its Way",            "في الطريق"),
                new TripStatus(TripStatuses.Arrived,             "Arrived",               "تم الوصول")
            };
        }
    }
}
