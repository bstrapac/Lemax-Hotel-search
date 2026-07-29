using System.Text.Json;

using HotelSearch.Models;

namespace HotelSearch.Utils
{
    public static class HotelLoad
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private static List<Hotel>? CachedHotels;

        private static readonly ILogger logger = LoggerFactory.Create(_ => { }).CreateLogger("HotelLoad");

        public static List<Hotel> LoadHotels()
        {
            if (CachedHotels is null)
            {
                CachedHotels = LoadHotelsFromFile();
            }

            return CachedHotels;
        }

        internal static void ReplaceCache(List<Hotel> hotels)
        {
            CachedHotels = hotels ?? [];
        }

        internal static void ResetCache()
        {
            CachedHotels = null;
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load hotels from mock JSON file");
                return [];
            }
        }
    }

    public static class HotelCreation
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private static readonly ILogger logger = LoggerFactory.Create(_ => { }).CreateLogger("HotelLoad");

        public static List<Hotel> CreateAndCacheHotels(string hotels)
        {
            if (string.IsNullOrWhiteSpace(hotels))
            {
                return [];
            }

            try
            {
                var createdHotels = JsonSerializer.Deserialize<List<Hotel>>(hotels, JsonOptions) ?? [];
                HotelLoad.ReplaceCache(createdHotels);
                return createdHotels;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create and cache hotels from imported JSON");
                return [];
            }
        }
    }
}
