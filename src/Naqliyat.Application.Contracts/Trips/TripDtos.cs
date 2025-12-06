using System;
using System.Collections.Generic;
using Naqliyat.Enums;
using Volo.Abp.Application.Dtos;

namespace Naqliyat.Trips;

public class TripDto : EntityDto<Guid>
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

    public TripStatuses StatusId { get; set; }

    public List<Guid> PictureIds { get; set; } = new();
}

public class CreateTripDto
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

    /// <summary>
    /// Attachment IDs for pictures associated with this trip.
    /// </summary>
    public List<Guid> PictureIds { get; set; } = new();
}

public class BidDto : EntityDto<Guid>
{
    public Guid TripId { get; set; }
    public Guid TruckId { get; set; }
    public double Price { get; set; }
    public DateTime ArrivalDate { get; set; }
    public BidStatus StatusId { get; set; }
}

public class CreateBidDto
{
    public Guid TruckId { get; set; }
    public double Price { get; set; }
    public DateTime ArrivalDate { get; set; }
}

public class PaymentDto : EntityDto<Guid>
{
    public Guid TripId { get; set; }
    public double PriceWithoutVat { get; set; }
    public string ReferenceId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public PaymentStatus Status { get; set; }
}

public class CreatePaymentDto
{
    public double PriceWithoutVat { get; set; }
}

public class ConfirmArrivalDto
{
    public string Otp { get; set; }
}

public class RateTripDto
{
    public int Rate { get; set; }
    public string Note { get; set; }
}

public interface ITripAppService
{
    System.Threading.Tasks.Task<TripDto> CreateAsync(CreateTripDto input);
    System.Threading.Tasks.Task<System.Collections.Generic.List<TripDto>> GetOpenTripsAsync(string locationFilter = null);
    System.Threading.Tasks.Task<TripDto> GetByIdAsync(System.Guid id);
    System.Threading.Tasks.Task<BidDto> PlaceBidAsync(System.Guid tripId, CreateBidDto input);
    System.Threading.Tasks.Task<System.Collections.Generic.List<BidDto>> GetNewBidsAsync(System.Guid tripId);
    System.Threading.Tasks.Task<BidDto> GetBidByIdAsync(System.Guid bidId);
    System.Threading.Tasks.Task<BidDto> ApproveBidAsync(System.Guid bidId);
    System.Threading.Tasks.Task<BidDto> RejectBidAsync(System.Guid bidId);
    System.Threading.Tasks.Task<PaymentDto> CreatePaymentAsync(System.Guid tripId, CreatePaymentDto input);
    System.Threading.Tasks.Task<TripDto> ConfirmArrivalAsync(System.Guid tripId, ConfirmArrivalDto input);
    System.Threading.Tasks.Task<TripDto> RateTripAsync(System.Guid tripId, RateTripDto input);
    System.Threading.Tasks.Task<System.Collections.Generic.List<TripDto>> GetDriverTripsAsync();
    System.Threading.Tasks.Task<TripDto> GetDriverTripByIdAsync(System.Guid tripId);
}
