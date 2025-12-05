using System;
using System.Collections.Generic;
using System.Text;
using Naqliyat.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trips
{
    public class Trip : FullAuditedAggregateRoot<Guid>
    {
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public double? GoodsWeight { get; set; }
        public string GoodsDimensions { get; set; }

        public TruckTypes? TruckTypeId { get; set; }
        public string GoodsType { get; set; }
        public string Notes { get; set; }

        public string Otp { get; set; }

        public TripStatuses StatusId { get; set; }
        public virtual TripStatus Status { get; set; }

        public virtual ICollection<TripPicture> TripPictures { get; set; }
        public virtual TripRating TripRating { get; set; }
        public virtual Payment Payment { get; set; }

        public Trip()
        {
            TripPictures = new HashSet<TripPicture>();
        }

        public Trip(
            Guid id,
            string fromLocation,
            string toLocation,
            DateTime startDate)
            : base(id)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
            StartDate = startDate;
            StatusId = TripStatuses.OpenForBidding;
            TripPictures = new HashSet<TripPicture>();
        }

        public Trip(
            Guid id,
            string fromLocation,
            string toLocation,
            DateTime startDate,
            DateTime? endDate,
            double? goodsWeight,
            string goodsDimensions,
            TruckTypes? truckTypeId,
            string goodsType,
            string notes)
            : this(id, fromLocation, toLocation, startDate)
        {
            EndDate = endDate;
            GoodsWeight = goodsWeight;
            GoodsDimensions = goodsDimensions;
            TruckTypeId = truckTypeId;
            GoodsType = goodsType;
            Notes = notes;
        }

        public void SetEndDate(DateTime? endDate)
        {
            EndDate = endDate;
        }

        public void SetGoodsDetails(double? weight, string dimensions, TruckTypes? truckTypeId, string goodsType)
        {
            GoodsWeight = weight;
            GoodsDimensions = dimensions;
            TruckTypeId = truckTypeId;
            GoodsType = goodsType;
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        public void SetOtp(string otp)
        {
            Otp = otp;
        }

        public void SetStatus(TripStatuses status)
        {
            StatusId = status;
        }

        public void SetRating(int rate, string note)
        {
            if (TripRating == null)
            {
                TripRating = new TripRating(Guid.NewGuid(), Id, rate, note);
            }
            else
            {
                TripRating.Rate = rate;
                TripRating.Note = note;
            }
        }

        public TripPicture AddPicture(Guid pictureId)
        {
            var picture = new TripPicture(Guid.NewGuid(), Id, pictureId);
            TripPictures.Add(picture);
            return picture;
        }
    }
}
