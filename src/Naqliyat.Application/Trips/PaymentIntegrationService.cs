using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Naqliyat.Trips
{
    public class PaymentIntegrationService : IPaymentIntegrationService, ITransientDependency
    {
        public Task<PaymentReservationResult> ReserveAsync(Guid tripId, double priceWithoutVat)
        {
            var result = new PaymentReservationResult
            {
                ReferenceId = Guid.NewGuid().ToString("N"),
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };

            return Task.FromResult(result);
        }
    }
}
