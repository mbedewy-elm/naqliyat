using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naqliyat.Trips;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Naqliyat.Trips;

public class TripAppService : NaqliyatAppService, ITripAppService
{
    private readonly IRepository<Trip, Guid> _tripRepository;
    private readonly IRepository<TripPicture, Guid> _tripPictureRepository;

    public TripAppService(
        IRepository<Trip, Guid> tripRepository,
        IRepository<TripPicture, Guid> tripPictureRepository)
    {
        _tripRepository = tripRepository;
        _tripPictureRepository = tripPictureRepository;
    }

    [UnitOfWork]
    public virtual async Task<TripDto> CreateAsync(CreateTripDto input)
    {
        var id = Guid.NewGuid();

        var trip = new Trip(
            id,
            input.FromLocation,
            input.ToLocation,
            input.StartDate,
            input.EndDate,
            input.GoodsWeight,
            input.GoodsDimensions,
            input.TruckTypeId,
            input.GoodsType,
            input.Notes
        );

        trip = await _tripRepository.InsertAsync(trip, autoSave: true);

        if (input.PictureIds != null && input.PictureIds.Any())
        {
            var pictures = new List<TripPicture>();

            foreach (var pictureId in input.PictureIds.Distinct())
            {
                pictures.Add(new TripPicture(Guid.NewGuid(), trip.Id, pictureId));
            }

            foreach (var pic in pictures)
            {
                await _tripPictureRepository.InsertAsync(pic, autoSave: true);
            }
        }

        // Load pictures if needed for DTO
        var pictureIds = input.PictureIds?.Distinct().ToList() ?? new List<Guid>();

        return new TripDto
        {
            Id = trip.Id,
            FromLocation = trip.FromLocation,
            ToLocation = trip.ToLocation,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            GoodsWeight = trip.GoodsWeight,
            GoodsDimensions = trip.GoodsDimensions,
            TruckTypeId = trip.TruckTypeId,
            GoodsType = trip.GoodsType,
            Notes = trip.Notes,
            StatusId = trip.StatusId,
            PictureIds = pictureIds
        };
    }
}
