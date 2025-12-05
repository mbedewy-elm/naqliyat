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

        public Task<bool> CompleteAsync(string referenceId)
        {
            // TODO: call external payment provider to verify/complete payment using referenceId
            // For now, assume completion is always successful.
            return Task.FromResult(true);
        }
    }
}
