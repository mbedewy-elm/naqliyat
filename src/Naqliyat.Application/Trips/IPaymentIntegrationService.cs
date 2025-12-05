using System;
using System.Threading.Tasks;

namespace Naqliyat.Trips
{
    public interface IPaymentIntegrationService
    {
        Task<PaymentReservationResult> ReserveAsync(Guid tripId, double priceWithoutVat);
        Task<bool> CompleteAsync(string referenceId);
    }

    public class PaymentReservationResult
    {
        public string ReferenceId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
