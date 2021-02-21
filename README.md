# docker-compose up --build

Docker compose will spawn a PosgreSQL and Redis containers along with 2 microservices:

1. Flight API - localhost:6003
2. Booking API - localhost:6001

Flow:
1. Flight.Api/initializeFlight will open the flight for bookings
2. Booking.Api/startBookingSession should be called to start a 5 minutes (configurable) session for the user to complete checkin.
  2.1 if the user doesn't complete the checkin during the session, a timeout exception will be thrown
3. Flight.Api/checkin should be called to finish the checkin to the flight
