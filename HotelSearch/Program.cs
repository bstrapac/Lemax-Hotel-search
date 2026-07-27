using HotelSearch.Services;
using HotelSearch.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

var hotelSearchService = new HotelSearchService();
Console.WriteLine(hotelSearchService.GetAllHotels()[0].Name);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/hotel_near_me", (Location location) =>
{
    var hotelSearchService = new HotelSearchService();
    return hotelSearchService.GetNearestHotels(location);
})
.WithName("GetHotelNearMe");

app.MapGet("/hotels", () =>
{
    var hotelSearchService = new HotelSearchService();
    return hotelSearchService.GetAllHotels();
})
.WithName("GetAllHotels");

app.Run();
