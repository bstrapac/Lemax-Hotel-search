using System.Linq;
using HotelSearch.Models;
using HotelSearch.Utils;

namespace HotelSearch.Services
{
    public class HotelSearchService
    {
        public Hotel? GetNearestHotel(Location location)
        {
            return HotelUtils.LoadHotels()
                .OrderBy(hotel => HotelUtils.GetDistance(location, hotel.Location))
                .FirstOrDefault();
        }

        public PagedHotelResult GetNearestHotelsPaged(Location location, int pageNumber = 1, int pageSize = 10)
        {
            var hotels = HotelUtils.LoadHotels()
                .OrderBy(hotel => HotelUtils.GetDistance(location, hotel.Location))
                .ThenBy(hotel => hotel.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

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
            return HotelUtils.LoadHotels();
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