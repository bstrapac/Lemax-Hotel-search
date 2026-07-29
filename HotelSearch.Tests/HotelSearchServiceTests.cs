using HotelSearch.Models;
using HotelSearch.Services;
using HotelSearch.Utils;

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

    [Fact]
    public void GetHotelsByPriceRange_ReturnsMatchingAndPagedResult()
    {
        var service = new HotelSearchService();

        var result = service.GetHotelsByPrice(200, 50, pageNumber: 1, pageSize: 5);

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count <= result.PageSize);
        Assert.All(result.Items, hotel => Assert.True(Math.Abs(hotel.Price - 200) <= 50));
    }

    [Fact]
    public void GetNearestHotelsPaged_AddsDistanceInKilometersForEachHotel()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 40.7128, Long = -74.0060 };

        var result = service.GetNearestHotelsPaged(location, pageNumber: 1, pageSize: 3);

        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, item => Assert.NotNull(item));
    }

    [Fact]
    public void GetNearestHotelsPaged_ReturnsHotelsFromClosestToFarthest()
    {
        var service = new HotelSearchService();
        var location = new Location { Lat = 40.6934, Long = -73.9939 };

        var result = service.GetNearestHotelsPaged(location, pageNumber: 1, pageSize: 5);

        Assert.NotNull(result.Items);
        Assert.NotEmpty(result.Items);
        Assert.Equal("Grand Hotel", result.Items[0].Name);
        Assert.True(result.Items.Zip(result.Items.Skip(1), (current, next) =>
        {
            var currentDistance = GetDistance(location, current.Location);
            var nextDistance = GetDistance(location, next.Location);
            return currentDistance <= nextDistance;
        }).All(x => x));
    }

    [Fact]
    public void GetHotelsByPrice_ReturnsHotelsFromClosestPriceToFarthest()
    {
        var service = new HotelSearchService();

        var result = service.GetHotelsByPrice(200, 50, pageNumber: 1, pageSize: 5);

        Assert.NotNull(result.Items);
        Assert.NotEmpty(result.Items);
        Assert.Equal(200, result.Items[0].Price);
        Assert.True(result.Items.Zip(result.Items.Skip(1), (current, next) =>
        {
            var currentDifference = Math.Abs(current.Price - 200);
            var nextDifference = Math.Abs(next.Price - 200);
            return currentDifference <= nextDifference;
        }).All(x => x));
    }

    [Fact]
    public void HotelCreation_CreatesAndCachesHotelsFromJsonPayload()
    {
        const string json = """
        [
          {
            "Name": "Imported Hotel",
            "Price": 123,
            "Location": {
              "Lat": 1.1,
              "Long": 2.2
            }
          }
        ]
        """;

        var hotels = HotelCreation.CreateAndCacheHotels(json);

        Assert.Single(hotels);
        Assert.Equal("Imported Hotel", hotels[0].Name);
        Assert.Equal(123, hotels[0].Price);
        Assert.Equal(1.1, hotels[0].Location.Lat);
        Assert.Equal(2.2, hotels[0].Location.Long);
    }

    private static double GetDistance(Location source, Location target)
    {
        var latDiff = source.Lat - target.Lat;
        var longDiff = source.Long - target.Long;
        return Math.Sqrt((latDiff * latDiff) + (longDiff * longDiff));
    }
}
