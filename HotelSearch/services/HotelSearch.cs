using HotelSearch.Models;
using HotelSearch.ApiClient;

namespace HotelSearch.Services
{
    public class HotelSearchService
    {
        public Hotel? GetNearestHotels(Location location)
        {
            var hotel = new Hotel("Test Name", 100, new Location { Lat = 40.7128, Long = -74.0060 });
            if (location.Lat == hotel.Location.Lat && location.Long == hotel.Location.Long)
            {
                return hotel;
            }
            else
            {
                return null;
            }
        }
        public List<Hotel> GetAllHotels()
        {
            return ApiClient.ApiClient.FetchHotelsFromOsmAsync(40.7128, -74.0060, 1000).Result;
        }
    }
}