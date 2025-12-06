using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Naqliyat.Enums;
using Volo.Abp.Application.Dtos;

namespace Naqliyat.Trucks;

public class TruckDto : EntityDto<Guid>
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string Year { get; set; }
    public string LicenseNumber { get; set; }
    public string RegistrationNumber { get; set; }
    public TruckTypes TruckTypeId { get; set; }
    public TemperatureRequirements TemperatureRequirement { get; set; }
    public Guid OwnerId { get; set; }
    public Guid? DriverId { get; set; }
    public double MaxLoad { get; set; }
    public List<Guid> PictureIds { get; set; } = new();
}

public class CreateTruckDto
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string Year { get; set; }
    public string LicenseNumber { get; set; }
    public string RegistrationNumber { get; set; }
    public TruckTypes TruckTypeId { get; set; }
    public TemperatureRequirements TemperatureRequirement { get; set; }
    public Guid OwnerId { get; set; }
    public Guid? DriverId { get; set; }
    public double MaxLoad { get; set; }
    public List<Guid> PictureIds { get; set; } = new();
}

public class DriverDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Identification { get; set; }
    public string LicenseNumber { get; set; }
    public Guid? PhotoId { get; set; }
    public int NationalityId { get; set; }
    public Guid UserId { get; set; }
}

public class CreateDriverDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Identification { get; set; }
    public string LicenseNumber { get; set; }
    public Guid? PhotoId { get; set; }
    public int NationalityId { get; set; }
    public Guid UserId { get; set; }
}

public interface ITruckAppService
{
    Task<TruckDto> CreateAsync(CreateTruckDto input);
    Task<List<TruckDto>> GetListAsync();
    Task<DriverDto> CreateDriverAsync(CreateDriverDto input);
    Task<List<DriverDto>> GetDriverListAsync();
    Task<DriverDto> GetDriverByIdAsync(Guid id);
    Task<DriverDto> GetDriverByUserIdAsync(Guid userId);
    Task<TruckDto> GetTruckByDriverIdAsync(Guid driverId);
}
