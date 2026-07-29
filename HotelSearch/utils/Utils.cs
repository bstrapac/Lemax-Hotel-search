using HotelSearch.Models;

namespace HotelSearch.Utils
{
    public static class HotelUtils
    {
        public static List<Hotel> LoadHotels()
        {
            return HotelLoad.LoadHotels();
        }

        const double earthRadiusKm = 6371.0;

        public static double GetDistance(Location source, Location target)
        {
            var latDiff = source.Lat - target.Lat;
            var longDiff = source.Long - target.Long;
            return Math.Sqrt((latDiff * latDiff) + (longDiff * longDiff));
        }

        public static double GetDistanceInKm(Location source, Location target)
        {
            var dLat = DegreesToRadians(target.Lat - source.Lat);
            var dLon = DegreesToRadians(target.Long - source.Long);

            var lat1 = DegreesToRadians(source.Lat);
            var lat2 = DegreesToRadians(target.Lat);

            // Haversine formula
            var sinDLat = Math.Sin(dLat / 2);
            var sinDLon = Math.Sin(dLon / 2);

            var a = (sinDLat * sinDLat) +
                    (Math.Cos(lat1) * Math.Cos(lat2) * sinDLon * sinDLon);
            a = Math.Clamp(a, 0.0, 1.0);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadiusKm * c;
        }

        public static HotelDto CreateHotelWithDistanceDto(Hotel hotel, Location source)
        {
            return new HotelDto
            {
                Hotel = hotel,
                DistanceKm = GetDistanceInKm(source, hotel.Location)
            };
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
