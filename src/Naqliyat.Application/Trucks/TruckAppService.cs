using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Naqliyat.Trucks;

public class TruckAppService : NaqliyatAppService, ITruckAppService
{
    private readonly IRepository<Truck, Guid> _truckRepository;
    private readonly IRepository<TruckPicture, Guid> _truckPictureRepository;
    private readonly IRepository<Driver, Guid> _driverRepository;

    public TruckAppService(
        IRepository<Truck, Guid> truckRepository,
        IRepository<TruckPicture, Guid> truckPictureRepository,
        IRepository<Driver, Guid> driverRepository)
    {
        _truckRepository = truckRepository;
        _truckPictureRepository = truckPictureRepository;
        _driverRepository = driverRepository;
    }

    [UnitOfWork]
    public virtual async Task<TruckDto> CreateAsync(CreateTruckDto input)
    {
        var id = Guid.NewGuid();

        var truck = new Truck(
            id,
            input.Make,
            input.Model,
            input.Year,
            input.LicenseNumber,
            input.RegistrationNumber,
            input.TruckTypeId,
            input.TemperatureRequirement,
            input.OwnerId,
            input.MaxLoad);

        if (input.DriverId.HasValue)
        {
            truck.SetDriver(input.DriverId.Value);
        }

        truck = await _truckRepository.InsertAsync(truck, autoSave: true);

        if (input.PictureIds != null && input.PictureIds.Any())
        {
            foreach (var pictureId in input.PictureIds.Distinct())
            {
                var truckPicture = new TruckPicture(Guid.NewGuid(), truck.Id, pictureId);
                await _truckPictureRepository.InsertAsync(truckPicture, autoSave: true);
            }
        }

        var pictureIds = input.PictureIds?.Distinct().ToList() ?? new List<Guid>();

        return new TruckDto
        {
            Id = truck.Id,
            Make = truck.Make,
            Model = truck.Model,
            Year = truck.Year,
            LicenseNumber = truck.LicenseNumber,
            RegistrationNumber = truck.RegistrationNumber,
            TruckTypeId = truck.TruckTypeId,
            TemperatureRequirement = truck.TemperatureRequirement,
            OwnerId = truck.OwnerId,
            DriverId = truck.DriverId == Guid.Empty ? null : truck.DriverId,
            MaxLoad = truck.MaxLoad,
            PictureIds = pictureIds
        };
    }

    [UnitOfWork]
    public virtual async Task<List<TruckDto>> GetListAsync()
    {
        var queryable = await _truckRepository.GetQueryableAsync();
        var trucks = await AsyncExecuter.ToListAsync(queryable);

        var truckIds = trucks.Select(t => t.Id).ToList();

        var pictureQueryable = await _truckPictureRepository.GetQueryableAsync();
        var truckPicturesQuery = pictureQueryable.Where(tp => truckIds.Contains(tp.TruckId));
        var truckPictures = await AsyncExecuter.ToListAsync(truckPicturesQuery);

        var picturesByTruck = truckPictures
            .GroupBy(tp => tp.TruckId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.PictureId).Distinct().ToList());

        var result = new List<TruckDto>();

        foreach (var truck in trucks)
        {
            picturesByTruck.TryGetValue(truck.Id, out var picIds);

            result.Add(new TruckDto
            {
                Id = truck.Id,
                Make = truck.Make,
                Model = truck.Model,
                Year = truck.Year,
                LicenseNumber = truck.LicenseNumber,
                RegistrationNumber = truck.RegistrationNumber,
                TruckTypeId = truck.TruckTypeId,
                TemperatureRequirement = truck.TemperatureRequirement,
                OwnerId = truck.OwnerId,
                DriverId = truck.DriverId == Guid.Empty ? null : truck.DriverId,
                MaxLoad = truck.MaxLoad,
                PictureIds = picIds ?? new List<Guid>()
            });
        }

        return result;
    }

    [UnitOfWork]
    public virtual async Task<DriverDto> CreateDriverAsync(CreateDriverDto input)
    {
        var id = Guid.NewGuid();

        Driver driver;
        if (input.PhotoId.HasValue)
        {
            driver = new Driver(
                id,
                input.Name,
                input.Phone,
                input.Identification,
                input.LicenseNumber,
                input.NationalityId,
                input.UserId,
                input.PhotoId.Value);
        }
        else
        {
            driver = new Driver(
                id,
                input.Name,
                input.Phone,
                input.Identification,
                input.LicenseNumber,
                input.NationalityId,
                input.UserId);
        }

        driver = await _driverRepository.InsertAsync(driver, autoSave: true);

        return new DriverDto
        {
            Id = driver.Id,
            Name = driver.Name,
            Phone = driver.Phone,
            Identification = driver.Identification,
            LicenseNumber = driver.LicenseNumber,
            PhotoId = driver.PhotoId == Guid.Empty ? null : driver.PhotoId,
            NationalityId = driver.NationalityId,
            UserId = driver.UserId
        };
    }

    [UnitOfWork]
    public virtual async Task<List<DriverDto>> GetDriverListAsync()
    {
        var queryable = await _driverRepository.GetQueryableAsync();
        var drivers = await AsyncExecuter.ToListAsync(queryable);

        return drivers.Select(driver => new DriverDto
        {
            Id = driver.Id,
            Name = driver.Name,
            Phone = driver.Phone,
            Identification = driver.Identification,
            LicenseNumber = driver.LicenseNumber,
            PhotoId = driver.PhotoId == Guid.Empty ? null : driver.PhotoId,
            NationalityId = driver.NationalityId,
            UserId = driver.UserId
        }).ToList();
    }

    [UnitOfWork]
    public virtual async Task<DriverDto> GetDriverByIdAsync(Guid id)
    {
        var driver = await _driverRepository.GetAsync(id);

        return new DriverDto
        {
            Id = driver.Id,
            Name = driver.Name,
            Phone = driver.Phone,
            Identification = driver.Identification,
            LicenseNumber = driver.LicenseNumber,
            PhotoId = driver.PhotoId == Guid.Empty ? null : driver.PhotoId,
            NationalityId = driver.NationalityId,
            UserId = driver.UserId
        };
    }

    [UnitOfWork]
    public virtual async Task<DriverDto> GetDriverByUserIdAsync(Guid userId)
    {
        var queryable = await _driverRepository.GetQueryableAsync();
        var driver = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(d => d.UserId == userId));

        if (driver == null)
        {
            return null;
        }

        return new DriverDto
        {
            Id = driver.Id,
            Name = driver.Name,
            Phone = driver.Phone,
            Identification = driver.Identification,
            LicenseNumber = driver.LicenseNumber,
            PhotoId = driver.PhotoId == Guid.Empty ? null : driver.PhotoId,
            NationalityId = driver.NationalityId,
            UserId = driver.UserId
        };
    }

    [UnitOfWork]
    public virtual async Task<TruckDto> GetTruckByDriverIdAsync(Guid driverId)
    {
        var queryable = await _truckRepository.GetQueryableAsync();
        var truck = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(t => t.DriverId == driverId));

        if (truck == null)
        {
            return null;
        }

        var pictureQueryable = await _truckPictureRepository.GetQueryableAsync();
        var truckPicturesQuery = pictureQueryable.Where(tp => tp.TruckId == truck.Id);
        var truckPictures = await AsyncExecuter.ToListAsync(truckPicturesQuery);
        var pictureIds = truckPictures.Select(tp => tp.PictureId).Distinct().ToList();

        return new TruckDto
        {
            Id = truck.Id,
            Make = truck.Make,
            Model = truck.Model,
            Year = truck.Year,
            LicenseNumber = truck.LicenseNumber,
            RegistrationNumber = truck.RegistrationNumber,
            TruckTypeId = truck.TruckTypeId,
            TemperatureRequirement = truck.TemperatureRequirement,
            OwnerId = truck.OwnerId,
            DriverId = truck.DriverId == Guid.Empty ? null : truck.DriverId,
            MaxLoad = truck.MaxLoad,
            PictureIds = pictureIds
        };
    }
}
