using AutoMapper;
using Flight.Api.Services;
using Flight.Api.ViewModel;
using Flight.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flight.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public FlightsController(IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IBookingService bookingService,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [Route("{id}")]
        [HttpGet]
        async public Task<ActionResult<Domain.Entities.Flight>> GetById(int id)
        {
            var flight = await _flightRepository.GetAsync(id);

            if (flight == null)
            {
                return NotFound();
            }    

            return Ok(flight);
        }

        [Route("initializeFlight")]
        [HttpPost]
        async public Task<ActionResult<string>> InitializeFlight([FromBody] InitializeFlightViewModel model)
        {
            var flight = await _flightRepository.GetAsync(model.FlightId);

            if (flight == null)
            {
                return NotFound();
            }

            var status = await _bookingService.InitializeFlight(flight.Id, flight.AvailableSeats);

            return Ok(status);
        }

        [Route("checkin")]
        [HttpPost]
        public async Task<ActionResult> Checkin([FromBody] CheckinViewModel model)
        {
            var flight = await _flightRepository.GetAsync(model.FlightId);
            var passenger = new Passenger(model.PassengerName, flight);

            
            model.Baggages.ForEach(b => {
                passenger.AddBaggage(b.Label, b.Weight);
            });
            
            var validationResponse = await _bookingService.ValidateAndEndBookingSessionAsync(model.FlightId, model.BookingSessionId);            
            _passengerRepository.Add(passenger);

            await _passengerRepository.UnitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
