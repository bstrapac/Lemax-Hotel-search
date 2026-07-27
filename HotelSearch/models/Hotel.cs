namespace HotelSearch.Models
{
    public class Hotel(string name, int price, Location location)
    {
        public string Name { get; set; } = name;
        public int Price { get; set; } = price;

        public Location Location { get; set; } = location;
    }

    public struct Location
    {
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
