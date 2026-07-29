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
        private readonly HotelSearchService _hotelSearchService;
        private readonly ILogger _logger;

        public HotelsController(ILogger<HotelsController> logger)
        {
            _logger = logger;
            _hotelSearchService = new HotelSearchService(logger);
        }

        [HttpGet]
        public ActionResult<List<Hotel>> GetAllHotels()
        {
            _logger.LogInformation("Retrieving all hotels");
            return Ok(_hotelSearchService.GetAllHotels());
        }

        [HttpGet("near-me")]
        public ActionResult<HotelDto?> GetNearestHotel([FromQuery] double lat, [FromQuery] double lng)
        {
            _logger.LogInformation("Searching nearest hotel for coordinates lat: {Lat}, lng: {Lng}", lat, lng);
            return Ok(_hotelSearchService.GetNearestHotel(new Location { Lat = lat, Long = lng }));
        }

        [HttpGet("near-me/paged")]
        public ActionResult<PagedHotelWithDistanceResult> GetNearestHotelsPaged([FromQuery] double lat, [FromQuery] double lng, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Searching paged nearby hotels for coordinates lat: {Lat}, lng: {Lng}, pageNumber: {PageNumber}, pageSize: {PageSize}", lat, lng, pageNumber, pageSize);
            return Ok(_hotelSearchService.GetNearestHotelsPaged(new Location { Lat = lat, Long = lng }, pageNumber, pageSize));
        }

        [HttpGet("price")]
        public ActionResult<PagedHotelResult> GetHotelsByPrice([FromQuery] int price, [FromQuery] int priceTolerance = 50, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Searching hotels by price: {Price} with tolerance {PriceTolerance}, pageNumber: {PageNumber}, pageSize: {PageSize}", price, priceTolerance, pageNumber, pageSize);
            return Ok(_hotelSearchService.GetHotelsByPrice(price, priceTolerance, pageNumber, pageSize));
        }

        [HttpPost("create")]
        public ActionResult<List<Hotel>> CreateHotelsFromJson([FromBody] string hotels)
        {
            _logger.LogInformation("Creating hotels from imported JSON payload");
            try
            {
                var createdHotels = HotelCreation.CreateAndCacheHotels(hotels);
                _logger.LogInformation("Imported {Count} hotels from JSON payload", createdHotels.Count);
                return Ok(createdHotels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process hotel import request");
                return StatusCode(500, new { message = "Failed to import hotels" });
            }
        }
    }
}
