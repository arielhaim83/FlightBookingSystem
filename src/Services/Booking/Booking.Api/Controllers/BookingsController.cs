using Booking.Api.Services;
using Booking.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;       

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;            
        }

        [Route("StartBookingSession")]
        [HttpPost]
        async public Task<ActionResult<string>> Initialize([FromBody] StartBookingSessionViewModel model)
        {            
            var bookingSessionId = await _bookingService.StartBookingSessionAsync(model.FlightId);            
            return Ok(new { bookingSessionId = bookingSessionId });
        }        
    }
}
