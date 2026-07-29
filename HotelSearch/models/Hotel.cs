namespace HotelSearch.Models
{
    public class Hotel(string name, int price, Location location)
    {
        public string Name { get; set; } = name;
        public int Price { get; set; } = price;

        public Location Location { get; set; } = location;
    }

    public class HotelDto
    {
        public Hotel Hotel { get; set; } = null!;
        public double DistanceKm { get; set; }
    }

    public class PagedHotelResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<Hotel> Items { get; set; } = [];
    }

    public class PagedHotelWithDistanceResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<HotelDto> Items { get; set; } = [];
    }

    public struct Location
    {
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
