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
            if (request.ParkingSpaceId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Parking space ID is required."
                });
            }

            var result =
                await _bookingService.CreateDayPassBookingAsync(
                    request,
                    User);

            if (!result.Success)
            {
                if (result.Error == "Parking space not found or inactive.")
                {
                    return NotFound(new
                    {
                        message = result.Error
                    });
                }

                if (result.Error ==
                    "Parking space is already occupied by an active booking." ||
                    result.Error ==
                    "Parking space is reserved by an active subscription.")
                {
                    return Conflict(new
                    {
                        message = result.Error
                    });
                }

                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                result.Booking);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message =
                    "An unexpected error occurred while creating the booking."
            });
        }
    }


    [HttpPost("{bookingId:guid}/checkout")]
    public async Task<IActionResult> Checkout(Guid bookingId)
    {
        try
        {
            if (bookingId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Booking ID is required."
                });
            }

            var result =
                await _bookingService.CheckoutBookingAsync(
                    bookingId,
                    User);

            if (!result.Success)
            {
                if (result.Error == "Booking not found.")
                {
                    return NotFound(new
                    {
                        message = result.Error
                    });
                }

                if (result.Error == "Booking is not active.")
                {
                    return Conflict(new
                    {
                        message = result.Error
                    });
                }

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
                message =
                    "An unexpected error occurred while checking out."
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