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
}
