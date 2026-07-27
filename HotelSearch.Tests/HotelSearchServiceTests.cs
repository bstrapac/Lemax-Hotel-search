using HotelSearch.Models;
using HotelSearch.Services;

namespace HotelSearch.Tests;

public class HotelSearchServiceTests
{
    [Fact]
    public void GetAllHotels_ReturnsTwoHotels()
    {
        var service = new HotelSearchService();

        var hotels = service.GetAllHotels();

        Assert.NotNull(hotels);
        Assert.Equal(2, hotels.Count);
        Assert.Contains(hotels, h => h.Name == "Test Name");
        Assert.Contains(hotels, h => h.Name == "Another Hotel");
    }

    [Fact]
    public void GetNearestHotels_ReturnsHotel_WhenLocationMatches()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 40.7128, Long = -74.0060 };

        var hotel = service.GetNearestHotels(location);

        Assert.NotNull(hotel);
        Assert.Equal("Test Name", hotel?.Name);
        Assert.Equal(100, hotel?.Price);
    }

    [Fact]
    public void GetNearestHotels_ReturnsNull_WhenLocationDoesNotMatch()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 0, Long = 0 };

        var hotel = service.GetNearestHotels(location);

        Assert.Null(hotel);
    }
}
