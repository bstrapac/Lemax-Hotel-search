using HotelSearch.Models;
using HotelSearch.Services;
using HotelSearch.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HotelSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly HotelSearchService _hotelSearchService = new();

        [HttpGet]
        public ActionResult<List<Hotel>> GetAllHotels()
        {
            return Ok(_hotelSearchService.GetAllHotels());
        }

        [HttpGet("near-me")]
        public ActionResult<Hotel?> GetNearestHotel([FromQuery] double lat, [FromQuery] double lng)
        {
            return Ok(_hotelSearchService.GetNearestHotel(new Location { Lat = lat, Long = lng }));
        }

        [HttpGet("near-me/paged")]
        public ActionResult<PagedHotelResult> GetNearestHotelsPaged([FromQuery] double lat, [FromQuery] double lng, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_hotelSearchService.GetNearestHotelsPaged(new Location { Lat = lat, Long = lng }, pageNumber, pageSize));
        }

        [HttpGet("price")]
        public ActionResult<PagedHotelResult> GetHotelsByPrice([FromQuery] int price, [FromQuery] int priceTolerance = 50, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_hotelSearchService.GetHotelsByPrice(price, priceTolerance, pageNumber, pageSize));
        }

        [HttpPost("create")]
        public ActionResult<List<Hotel>> CreateHotelsFromJson([FromBody] string hotels)
        {
            var createdHotels = HotelCreation.CreateAndCacheHotels(hotels);
            return Ok(createdHotels);
        }
    }
}
