using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking_Management.Server.DTOs.Booking;
using Parking_Management.Server.Services;

namespace Parking_Management.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(
        CreateBookingRequest request)
    {
        try
        {
            var result =
                await _bookingService.CreateDayPassBookingAsync(
                    request,
                    User);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(result.Booking);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred while creating the booking."
            });
        }
    }


    [HttpPost("{bookingId:guid}/checkout")]
    public async Task<IActionResult> Checkout(Guid bookingId)
    {
        try
        {
            var result = await _bookingService.CheckoutBookingAsync(
                bookingId,
                User);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(result.Booking);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred while checking out."
            });
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        try
        {
            var result = await _bookingService.GetMyBookingsAsync(User);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(result.Bookings);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred while retrieving bookings."
            });
        }
    }
}