using HotelSearch.Models;
using HotelSearch.Services;

namespace HotelSearch.Tests;

public class HotelSearchServiceTests
{
    [Fact]
    public void GetAllHotels_ReturnsHotelsFromJsonFile()
    {
        var service = new HotelSearchService();

        var hotels = service.GetAllHotels();

        Assert.NotNull(hotels);
        Assert.NotEmpty(hotels);
    }

    [Fact]
    public void GetNearestHotels_ReturnsHotel_WhenLocationIsProvided()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 40.7128, Long = -74.0060 };

        var hotel = service.GetNearestHotel(location);

        Assert.NotNull(hotel);
    }

    [Fact]
    public void GetNearestHotels_ReturnsClosestHotel_WhenLocationIsFarAway()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 0, Long = 0 };

        var hotel = service.GetNearestHotel(location);

        Assert.NotNull(hotel);
    }

    [Fact]
    public void GetNearestHotelsPaged_ReturnsPagedResult()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 40.7128, Long = -74.0060 };

        var result = service.GetNearestHotelsPaged(location, pageNumber: 1, pageSize: 5);

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.True(result.TotalCount >= 0);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count <= result.PageSize);
    }
}
