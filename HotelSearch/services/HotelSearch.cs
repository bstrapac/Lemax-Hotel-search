using HotelSearch.Models;
using HotelSearch.Utils;

namespace HotelSearch.Services
{
    public class HotelSearchService
    {
        private readonly ILogger _logger;

        public HotelSearchService(ILogger? logger = null)
        {
            _logger = logger ?? LoggerFactory.Create(_ => { }).CreateLogger("HotelSearchService");
        }

        public Hotel? GetNearestHotel(Location location)
        {
            var hotels = HotelUtils.LoadHotels();
            var nearestHotel = hotels
                .OrderBy(hotel => HotelUtils.GetDistance(location, hotel.Location))
                .FirstOrDefault();

            _logger.LogInformation("Resolved nearest hotel for lat: {Lat}, lng: {Lng}. Result: {HotelName}", location.Lat, location.Long, nearestHotel?.Name ?? "None");
            return nearestHotel;
        }

        public PagedHotelResult GetNearestHotelsPaged(Location location, int pageNumber = 1, int pageSize = 10)
        {
            var hotels = HotelUtils.LoadHotels()
                .OrderBy(hotel => HotelUtils.GetDistance(location, hotel.Location))
                .ThenBy(hotel => hotel.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            _logger.LogInformation("Returning {Count} nearby hotels for lat: {Lat}, lng: {Lng}, pageNumber: {PageNumber}, pageSize: {PageSize}", hotels.Count, location.Lat, location.Long, pageNumber, pageSize);

            var safePageNumber = pageNumber < 1 ? 1 : pageNumber;
            var safePageSize = pageSize < 1 ? 10 : pageSize;
            var startIndex = (safePageNumber - 1) * safePageSize;

            return new PagedHotelResult
            {
                PageNumber = safePageNumber,
                PageSize = safePageSize,
                TotalCount = hotels.Count,
                Items = hotels.Skip(startIndex).Take(safePageSize).ToList()
            };
        }

        public List<Hotel> GetAllHotels()
        {
            var hotels = HotelUtils.LoadHotels();
            _logger.LogInformation("Returning {Count} hotels from the active source", hotels.Count);
            return hotels;
        }

        public PagedHotelResult GetHotelsByPrice(int targetPrice, int priceTolerance = 50, int pageNumber = 1, int pageSize = 10)
        {
            var safeTolerance = priceTolerance < 0 ? 0 : priceTolerance;
            var safePageNumber = pageNumber < 1 ? 1 : pageNumber;
            var safePageSize = pageSize < 1 ? 10 : pageSize;
            var startIndex = (safePageNumber - 1) * safePageSize;

            var hotels = HotelUtils.LoadHotels()
                .Where(hotel => Math.Abs(hotel.Price - targetPrice) <= safeTolerance)
                .OrderBy(hotel => Math.Abs(hotel.Price - targetPrice))
                .ThenBy(hotel => hotel.Price)
                .ThenBy(hotel => hotel.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            _logger.LogInformation("Returning {Count} hotels within price tolerance {Tolerance} for target price {TargetPrice}", hotels.Count, safeTolerance, targetPrice);

            return new PagedHotelResult
            {
                PageNumber = safePageNumber,
                PageSize = safePageSize,
                TotalCount = hotels.Count,
                Items = hotels.Skip(startIndex).Take(safePageSize).ToList()
            };
        }

    }
}
