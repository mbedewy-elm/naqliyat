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
}
