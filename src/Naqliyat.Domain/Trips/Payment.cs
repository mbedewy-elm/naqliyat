using System;
using Naqliyat.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class Payment : AuditedEntity<Guid>
    {
        public Guid TripId { get; set; }

        public double PriceWithoutVat { get; set; }
        public string ReferenceId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public PaymentStatus Status { get; set; }

        public virtual Trip Trip { get; set; }

        public Payment()
        {
            Status = PaymentStatus.Pending;
        }

        public Payment(
            Guid id,
            Guid tripId,
            double priceWithoutVat,
            string referenceId,
            DateTime expiryDate)
            : base(id)
        {
            TripId = tripId;
            PriceWithoutVat = priceWithoutVat;
            ReferenceId = referenceId;
            ExpiryDate = expiryDate;
            Status = PaymentStatus.Pending;
        }

        public void MarkPaid()
        {
            Status = PaymentStatus.Paid;
        }

        public void Cancel()
        {
            Status = PaymentStatus.Cancelled;
        }

        public void MarkExpired()
        {
            Status = PaymentStatus.Expired;
        }
    }
}
