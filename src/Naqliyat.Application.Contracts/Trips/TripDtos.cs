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

public interface ITripAppService
{
    System.Threading.Tasks.Task<TripDto> CreateAsync(CreateTripDto input);
}
