namespace HotelSearch.Models
{
    public class Hotel(string name, int price, Location location)
    {
        public string Name { get; set; } = name;
        public int Price { get; set; } = price;
        public Location Location { get; set; } = location;
    }

    public class HotelDto(Hotel hotel, double distanceKm)
    {
        public Hotel Hotel { get; set; } = hotel;
        public double DistanceKm { get; set; } = distanceKm;
    }

    public class PagedHotelResult(int pageNumber, int pageSize, int totalCount, List<Hotel> items)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
        public List<Hotel> Items { get; set; } = items;
    }

    public class PagedHotelWithDistanceResult(int pageNumber, int pageSize, int totalCount, List<HotelDto> items)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
        public List<HotelDto> Items { get; set; } = items;
    }

    public struct Location
    {
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
