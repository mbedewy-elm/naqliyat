using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naqliyat.Permissions;
using Naqliyat.Trucks;

namespace Naqliyat.Controllers.Trucks;

[Route("api/app/trucks")]
public class TruckController : NaqliyatController
{
    private readonly ITruckAppService _truckAppService;

    public TruckController(ITruckAppService truckAppService)
    {
        _truckAppService = truckAppService;
    }

    [HttpPost]
    [Authorize(NaqliyatPermissions.Trucks.Create)]
    public virtual Task<TruckDto> CreateAsync([FromBody] CreateTruckDto input)
    {
        return _truckAppService.CreateAsync(input);
    }

    [HttpGet]
    [Authorize(NaqliyatPermissions.Trucks.List)]
    public virtual Task<List<TruckDto>> GetListAsync()
    {
        return _truckAppService.GetListAsync();
    }

    [HttpPost("drivers")]
    [Authorize(NaqliyatPermissions.Drivers.Create)]
    public virtual Task<DriverDto> CreateDriverAsync([FromBody] CreateDriverDto input)
    {
        return _truckAppService.CreateDriverAsync(input);
    }

    [HttpGet("drivers")]
    [Authorize(NaqliyatPermissions.Drivers.List)]
    public virtual Task<List<DriverDto>> GetDriverListAsync()
    {
        return _truckAppService.GetDriverListAsync();
    }

    [HttpGet("drivers/{id}")]
    [Authorize(NaqliyatPermissions.Drivers.ViewDetails)]
    public virtual Task<DriverDto> GetDriverByIdAsync(Guid id)
    {
        return _truckAppService.GetDriverByIdAsync(id);
    }

    [HttpGet("drivers/by-user/{userId}")]
    [Authorize(NaqliyatPermissions.Drivers.ViewDetails)]
    public virtual Task<DriverDto> GetDriverByUserIdAsync(Guid userId)
    {
        return _truckAppService.GetDriverByUserIdAsync(userId);
    }

    [HttpGet("by-driver/{driverId}")]
    [Authorize(NaqliyatPermissions.Trucks.List)]
    public virtual Task<TruckDto> GetTruckByDriverIdAsync(Guid driverId)
    {
        return _truckAppService.GetTruckByDriverIdAsync(driverId);
    }
}
