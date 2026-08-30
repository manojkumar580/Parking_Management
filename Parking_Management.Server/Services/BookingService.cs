using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Data;
using Parking_Management.Server.DTOs.Booking;
using Parking_Management.Server.Models;

namespace Parking_Management.Server.Services;

public class BookingService
{
    private readonly ParkingManagementDbContext _context;
    private readonly IConfiguration _configuration;

    public BookingService(ParkingManagementDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool Success, string? Error, BookingResponse? Booking)>
        CreateDayPassBookingAsync(
            CreateBookingRequest request,
            ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, "Invalid user authentication.", null);
            }

            var parkingSpace = await _context.ParkingSpaces
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ParkingSpaceId &&
                    x.IsActive);

            if (parkingSpace == null)
            {
                return (false, "Parking space not found or inactive.", null);
            }

            var spaceHasActiveBooking = await _context.Bookings
            .AnyAsync(x =>
                x.ParkingSpaceId == request.ParkingSpaceId &&
                x.Status == BookingStatus.Active);

            if (spaceHasActiveBooking)
            {
                return (false, "Parking space is already occupied by an active booking.", null);
            }

            var spaceHasActiveSubscription = await _context.Subscriptions
                .AnyAsync(x =>
                    x.ParkingSpaceId == request.ParkingSpaceId &&
                    x.Status == SubscriptionStatus.Active &&
                    x.StartDate <= DateTime.UtcNow &&
                    x.EndDate > DateTime.UtcNow);

            if (spaceHasActiveSubscription)
            {
                return (
                    false,
                    "Parking space is reserved by an active subscription.",
                    null);
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ParkingSpaceId = parkingSpace.Id,
                CheckInTime = DateTime.UtcNow,
                CheckOutTime = null,
                Amount = null,
                Status = BookingStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            var response = new BookingResponse
            {
                Id = booking.Id,
                ParkingSpaceId = parkingSpace.Id,
                SpaceNumber = parkingSpace.SpaceNumber,
                CheckInTime = booking.CheckInTime,
                CheckOutTime = booking.CheckOutTime,
                Amount = booking.Amount,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt
            };

            return (true, null, response);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while creating the booking.",
                ex);
        }
    }

    public async Task<(bool Success, string? Error, BookingResponse? Booking)>
    CheckoutBookingAsync(
        Guid bookingId,
        ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, "Invalid user authentication.", null);
            }

            var booking = await _context.Bookings
                .Include(x => x.ParkingSpace)
                .FirstOrDefaultAsync(x =>
                    x.Id == bookingId &&
                    x.UserId == userId);

            if (booking == null)
            {
                return (false, "Booking not found.", null);
            }

            if (booking.Status != BookingStatus.Active)
            {
                return (false, "Booking is not active.", null);
            }

            var checkOutTime = DateTime.UtcNow;

            if (checkOutTime <= booking.CheckInTime)
            {
                return (false, "Check-out time must be after check-in time.", null);
            }

            var duration = checkOutTime - booking.CheckInTime;

            var totalHours = (int)Math.Ceiling(
                duration.TotalHours);

            var first24HoursRate =
                _configuration.GetValue<decimal>(
                    "ParkingPricing:First24HoursRate");

            var after24HoursRate =
                _configuration.GetValue<decimal>(
                    "ParkingPricing:After24HoursRate");

            if (first24HoursRate <= 0)
            {
                return (
                    false,
                    "First 24 hours parking rate is not configured correctly.",
                    null);
            }

            if (after24HoursRate <= 0)
            {
                return (
                    false,
                    "After 24 hours parking rate is not configured correctly.",
                    null);
            }

            decimal amount;

            if (duration.TotalHours <= 24)
            {
                amount = totalHours * first24HoursRate;
            }
            else
            {
                var first24Hours = 24;

                var remainingHours =
                    totalHours - first24Hours;

                amount =
                    (first24Hours * first24HoursRate) +
                    (remainingHours * after24HoursRate);
            }

            booking.CheckOutTime = checkOutTime;
            booking.Amount = amount;
            booking.Status = BookingStatus.Completed;

            await _context.SaveChangesAsync();

            var response = new BookingResponse
            {
                Id = booking.Id,
                ParkingSpaceId = booking.ParkingSpaceId,
                SpaceNumber = booking.ParkingSpace.SpaceNumber,
                CheckInTime = booking.CheckInTime,
                CheckOutTime = booking.CheckOutTime,
                Amount = booking.Amount,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt
            };

            return (true, null, response);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while checking out the booking.",
                ex);
        }
    }

    public async Task<(bool Success, string? Error, List<BookingResponse>? Bookings)>
    GetMyBookingsAsync(ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, "Invalid user authentication.", null);
            }

            var bookings = await _context.Bookings
                .Include(x => x.ParkingSpace)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new BookingResponse
                {
                    Id = x.Id,
                    ParkingSpaceId = x.ParkingSpaceId,
                    SpaceNumber = x.ParkingSpace.SpaceNumber,
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime,
                    Amount = x.Amount,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return (true, null, bookings);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while retrieving the user's bookings.",
                ex);
        }
    }
}