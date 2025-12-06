using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naqliyat.Permissions;
using Naqliyat.Trips;
using Volo.Abp.Authorization;

namespace Naqliyat.Controllers.Trips;

[Route("api/app/trips")]
public class TripController : NaqliyatController
{
    private readonly ITripAppService _tripAppService;

    public TripController(ITripAppService tripAppService)
    {
        _tripAppService = tripAppService;
    }

    [HttpPost]
    [Authorize(NaqliyatPermissions.Trips.Create)]
    public virtual Task<TripDto> CreateAsync([FromBody] CreateTripDto input)
    {
        return _tripAppService.CreateAsync(input);
    }

    [HttpGet("open")]
    [Authorize(NaqliyatPermissions.Trips.ViewOpen)]
    public virtual Task<System.Collections.Generic.List<TripDto>> GetOpenAsync([FromQuery] string locationFilter = null)
    {
        return _tripAppService.GetOpenTripsAsync(locationFilter);
    }

    [HttpGet("{id}")]
    [Authorize(NaqliyatPermissions.Trips.ViewDetails)]
    public virtual Task<TripDto> GetByIdAsync(System.Guid id)
    {
        return _tripAppService.GetByIdAsync(id);
    }

    [HttpPost("{id}/bids")]
    [Authorize(NaqliyatPermissions.Trips.Bid)]
    public virtual Task<BidDto> PlaceBidAsync(System.Guid id, [FromBody] CreateBidDto input)
    {
        return _tripAppService.PlaceBidAsync(id, input);
    }

    [HttpGet("{id}/bids/new")]
    [Authorize(NaqliyatPermissions.Trips.ViewBids)]
    public virtual Task<System.Collections.Generic.List<BidDto>> GetNewBidsAsync(System.Guid id)
    {
        return _tripAppService.GetNewBidsAsync(id);
    }

    [HttpGet("bids/{bidId}")]
    [Authorize(NaqliyatPermissions.Trips.ViewBids)]
    public virtual Task<BidDto> GetBidByIdAsync(System.Guid bidId)
    {
        return _tripAppService.GetBidByIdAsync(bidId);
    }

    [HttpPost("bids/{bidId}/approve")]
    [Authorize(NaqliyatPermissions.Trips.ManageBids)]
    public virtual Task<BidDto> ApproveBidAsync(System.Guid bidId)
    {
        return _tripAppService.ApproveBidAsync(bidId);
    }

    [HttpPost("bids/{bidId}/reject")]
    [Authorize(NaqliyatPermissions.Trips.ManageBids)]
    public virtual Task<BidDto> RejectBidAsync(System.Guid bidId)
    {
        return _tripAppService.RejectBidAsync(bidId);
    }

    [HttpPost("{id}/payments")]
    [Authorize(NaqliyatPermissions.Trips.CreatePayment)]
    public virtual Task<PaymentDto> CreatePaymentAsync(System.Guid id, [FromBody] CreatePaymentDto input)
    {
        return _tripAppService.CreatePaymentAsync(id, input);
    }

    [HttpPost("{id}/confirm-arrival")]
    [Authorize(NaqliyatPermissions.Trips.ConfirmArrival)]
    public virtual Task<TripDto> ConfirmArrivalAsync(System.Guid id, [FromBody] ConfirmArrivalDto input)
    {
        return _tripAppService.ConfirmArrivalAsync(id, input);
    }

    [HttpPost("{id}/rating")]
    [Authorize(NaqliyatPermissions.Trips.Rate)]
    public virtual Task<TripDto> RateTripAsync(System.Guid id, [FromBody] RateTripDto input)
    {
        return _tripAppService.RateTripAsync(id, input);
    }

    [HttpGet("driver/my-trips")]
    [Authorize(NaqliyatPermissions.Trips.DriverTrips)]
    public virtual Task<System.Collections.Generic.List<TripDto>> GetDriverTripsAsync()
    {
        return _tripAppService.GetDriverTripsAsync();
    }

    [HttpGet("driver/my-trips/{tripId}")]
    [Authorize(NaqliyatPermissions.Trips.DriverTrips)]
    public virtual Task<TripDto> GetDriverTripByIdAsync(System.Guid tripId)
    {
        return _tripAppService.GetDriverTripByIdAsync(tripId);
    }
}
