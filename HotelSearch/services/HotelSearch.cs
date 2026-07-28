using System.IO;
using System.Linq;
using System.Text.Json;
using HotelSearch.Models;

namespace HotelSearch.Services
{
    public class HotelSearchService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly Lazy<List<Hotel>> CachedHotels = new(
            () => LoadHotelsFromFile(),
            LazyThreadSafetyMode.ExecutionAndPublication);

        public Hotel? GetNearestHotel(Location location)
        {
            return LoadHotels()
                .OrderBy(hotel => GetDistance(location, hotel.Location))
                .FirstOrDefault();
        }

        public PagedHotelResult GetNearestHotelsPaged(Location location, int pageNumber = 1, int pageSize = 10)
        {
            var hotels = LoadHotels()
                .OrderBy(hotel => GetDistance(location, hotel.Location))
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
            return LoadHotels();
        }

        private static List<Hotel> LoadHotels()
        {
            return CachedHotels.Value;
        }

        private static List<Hotel> LoadHotelsFromFile()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "mock-hotels.json");
                if (!File.Exists(filePath))
                {
                    return [];
                }

                var json = File.ReadAllText(filePath);
                var hotels = JsonSerializer.Deserialize<List<Hotel>>(json, JsonOptions);

                return hotels ?? [];
            }
            catch (Exception)
            {
                return [];
            }
        }

        private static double GetDistance(Location source, Location target)
        {
            var latDiff = source.Lat - target.Lat;
            var longDiff = source.Long - target.Long;
            return Math.Sqrt((latDiff * latDiff) + (longDiff * longDiff));
        }
    }
}