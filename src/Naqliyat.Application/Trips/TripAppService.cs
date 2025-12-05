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
    private readonly IRepository<Bid, Guid> _bidRepository;
    private readonly IRepository<Payment, Guid> _paymentRepository;
    private readonly IPaymentIntegrationService _paymentIntegrationService;

    public TripAppService(
        IRepository<Trip, Guid> tripRepository,
        IRepository<TripPicture, Guid> tripPictureRepository,
        IRepository<Bid, Guid> bidRepository,
        IRepository<Payment, Guid> paymentRepository,
        IPaymentIntegrationService paymentIntegrationService)
    {
        _tripRepository = tripRepository;
        _tripPictureRepository = tripPictureRepository;
        _bidRepository = bidRepository;
        _paymentRepository = paymentRepository;
        _paymentIntegrationService = paymentIntegrationService;
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

    [UnitOfWork]
    public virtual async Task<BidDto> PlaceBidAsync(Guid tripId, CreateBidDto input)
    {
        // Ensure trip exists
        var trip = await _tripRepository.GetAsync(tripId);

        var bid = new Bid(tripId, input.TruckId, input.Price, input.ArrivalDate);

        bid = await _bidRepository.InsertAsync(bid, autoSave: true);

        return new BidDto
        {
            Id = bid.Id,
            TripId = bid.TripId,
            TruckId = bid.TruckId,
            Price = bid.Price,
            ArrivalDate = bid.ArrivalDate,
            StatusId = bid.StatusId
        };
    }

    [UnitOfWork]
    public virtual async Task<List<BidDto>> GetNewBidsAsync(Guid tripId)
    {
        var queryable = await _bidRepository.GetQueryableAsync();

        var query = queryable
            .Where(b => b.TripId == tripId && b.StatusId == BidStatus.New);

        var bids = await AsyncExecuter.ToListAsync(query);

        return bids.Select(b => new BidDto
        {
            Id = b.Id,
            TripId = b.TripId,
            TruckId = b.TruckId,
            Price = b.Price,
            ArrivalDate = b.ArrivalDate,
            StatusId = b.StatusId
        }).ToList();
    }

    [UnitOfWork]
    public virtual async Task<BidDto> GetBidByIdAsync(Guid bidId)
    {
        var bid = await _bidRepository.GetAsync(bidId);

        return new BidDto
        {
            Id = bid.Id,
            TripId = bid.TripId,
            TruckId = bid.TruckId,
            Price = bid.Price,
            ArrivalDate = bid.ArrivalDate,
            StatusId = bid.StatusId
        };
    }

    [UnitOfWork]
    public virtual async Task<BidDto> ApproveBidAsync(Guid bidId)
    {
        var bid = await _bidRepository.GetAsync(bidId);

        bid.Accept();

        bid = await _bidRepository.UpdateAsync(bid, autoSave: true);

        return new BidDto
        {
            Id = bid.Id,
            TripId = bid.TripId,
            TruckId = bid.TruckId,
            Price = bid.Price,
            ArrivalDate = bid.ArrivalDate,
            StatusId = bid.StatusId
        };
    }

    [UnitOfWork]
    public virtual async Task<BidDto> RejectBidAsync(Guid bidId)
    {
        var bid = await _bidRepository.GetAsync(bidId);

        bid.Reject();

        bid = await _bidRepository.UpdateAsync(bid, autoSave: true);

        return new BidDto
        {
            Id = bid.Id,
            TripId = bid.TripId,
            TruckId = bid.TruckId,
            Price = bid.Price,
            ArrivalDate = bid.ArrivalDate,
            StatusId = bid.StatusId
        };
    }

    [UnitOfWork]
    public virtual async Task<PaymentDto> CreatePaymentAsync(Guid tripId, CreatePaymentDto input)
    {
        var trip = await _tripRepository.GetAsync(tripId);

        var integrationResult = await _paymentIntegrationService.ReserveAsync(tripId, input.PriceWithoutVat);

        var payment = new Payment(
            Guid.NewGuid(),
            tripId,
            input.PriceWithoutVat,
            integrationResult.ReferenceId,
            integrationResult.ExpiryDate);

        payment = await _paymentRepository.InsertAsync(payment, autoSave: true);

        return new PaymentDto
        {
            Id = payment.Id,
            TripId = payment.TripId,
            PriceWithoutVat = payment.PriceWithoutVat,
            ReferenceId = payment.ReferenceId,
            ExpiryDate = payment.ExpiryDate,
            Status = payment.Status
        };
    }
}
