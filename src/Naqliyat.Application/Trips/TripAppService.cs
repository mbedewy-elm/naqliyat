using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naqliyat.Enums;
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

    [UnitOfWork]
    public virtual async Task<List<TripDto>> GetOpenTripsAsync(string locationFilter = null)
    {
        var queryable = await _tripRepository.GetQueryableAsync();

        var query = queryable
            .Where(t => t.StatusId == TripStatuses.OpenForBidding);

        if (!string.IsNullOrWhiteSpace(locationFilter))
        {
            var filter = locationFilter.Trim();
            query = query.Where(t =>
                t.FromLocation.Contains(filter) ||
                t.ToLocation.Contains(filter));
        }

        var trips = await AsyncExecuter.ToListAsync(query);

        var tripIds = trips.Select(t => t.Id).ToList();

        var pictureQueryable = await _tripPictureRepository.GetQueryableAsync();
        var tripPicturesQuery = pictureQueryable
            .Where(tp => tripIds.Contains(tp.TripId));

        var tripPictures = await AsyncExecuter.ToListAsync(tripPicturesQuery);

        var picturesByTrip = tripPictures
            .GroupBy(tp => tp.TripId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.PictureId).Distinct().ToList());

        var result = new List<TripDto>();

        foreach (var trip in trips)
        {
            picturesByTrip.TryGetValue(trip.Id, out var picIds);

            result.Add(new TripDto
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
                PictureIds = picIds ?? new List<Guid>()
            });
        }

        return result;
    }

    [UnitOfWork]
    public virtual async Task<TripDto> GetByIdAsync(Guid id)
    {
        var trip = await _tripRepository.GetAsync(id);

        var pictureQueryable = await _tripPictureRepository.GetQueryableAsync();
        var tripPicturesQuery = pictureQueryable
            .Where(tp => tp.TripId == id);

        var tripPictures = await AsyncExecuter.ToListAsync(tripPicturesQuery);

        var pictureIds = tripPictures
            .Select(tp => tp.PictureId)
            .Distinct()
            .ToList();

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
