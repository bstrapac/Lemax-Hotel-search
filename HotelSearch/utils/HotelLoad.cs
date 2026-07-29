using System.IO;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return [];
            }
        }
    }
}
