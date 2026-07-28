using HotelSearch.Services;
using HotelSearch.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

var hotelSearchService = new HotelSearchService();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hotel_near_me", (double lat, double lng) =>
{
    return hotelSearchService.GetNearestHotel(new Location { Lat = lat, Long = lng });
})
.WithName("GetHotelNearMe");

app.MapGet("/hotels_near_me/paged", (double lat, double lng, int pageNumber = 1, int pageSize = 10) =>
{
    return hotelSearchService.GetNearestHotelsPaged(new Location { Lat = lat, Long = lng }, pageNumber, pageSize);
})
.WithName("GetHotelsNearMePaged");

app.MapGet("/hotels", () =>
{
    return hotelSearchService.GetAllHotels();
})
.WithName("GetAllHotels");

app.Run();
