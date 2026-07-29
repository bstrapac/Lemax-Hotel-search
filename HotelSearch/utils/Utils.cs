using HotelSearch.Models;

namespace HotelSearch.Utils
{
    public static class HotelUtils
    {
        public static List<Hotel> LoadHotels()
        {
            return HotelLoad.LoadHotels();
        }

        public static double GetDistance(Location source, Location target)
        {
            var latDiff = source.Lat - target.Lat;
            var longDiff = source.Long - target.Long;
            return Math.Sqrt((latDiff * latDiff) + (longDiff * longDiff));
        }
    }
}
