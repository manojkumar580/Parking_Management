using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Data;
using Parking_Management.Server.DTOs.Subscription;
using Parking_Management.Server.Models;

namespace Parking_Management.Server.Services;

public class SubscriptionService
{
    private readonly ParkingManagementDbContext _context;
    private readonly IConfiguration _configuration;

    public SubscriptionService(ParkingManagementDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool Success, string? Error, SubscriptionResponse? Subscription)>
        CreateSubscriptionAsync(
            CreateSubscriptionRequest request,
            ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                return (
                    false,
                    "Parking space not found or inactive.",
                    null);
            }

            var today = DateTime.UtcNow.Date;
            var endDate = today.AddMonths(1);

            var spaceAlreadySubscribed =
                await _context.Subscriptions.AnyAsync(x =>
                    x.ParkingSpaceId == request.ParkingSpaceId &&
                    x.Status == SubscriptionStatus.Active &&
                    x.StartDate < endDate &&
                    x.EndDate > today);

            if (spaceAlreadySubscribed)
            {
                return (
                    false,
                    "Parking space is already reserved by an active subscription.",
                    null);
            }

            var spaceHasActiveBooking = await _context.Bookings
            .AnyAsync(x =>
                x.ParkingSpaceId == request.ParkingSpaceId &&
                x.Status == BookingStatus.Active);

            if (spaceHasActiveBooking)
            {
                return (
                    false,
                    "Parking space is currently occupied by an active booking.",
                    null);
            }

            var monthlySubscriptionRate =
                _configuration.GetValue<decimal>(
                    "ParkingPricing:MonthlySubscriptionRate");

            if (monthlySubscriptionRate <= 0)
            {
                return (
                    false,
                    "Monthly subscription rate is not configured correctly.",
                    null);
            }

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ParkingSpaceId = parkingSpace.Id,
                StartDate = today,
                EndDate = endDate,
                Amount = monthlySubscriptionRate,
                Status = SubscriptionStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Subscriptions.Add(subscription);

            await _context.SaveChangesAsync();

            var response = new SubscriptionResponse
            {
                Id = subscription.Id,
                ParkingSpaceId = subscription.ParkingSpaceId,
                SpaceNumber = parkingSpace.SpaceNumber,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                Amount = subscription.Amount,
                Status = subscription.Status.ToString(),
                CreatedAt = subscription.CreatedAt
            };

            return (true, null, response);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while creating the subscription.",
                ex);
        }
    }

    public async Task<
    (bool Success, string? Error, List<SubscriptionResponse>? Subscriptions)>
    GetMySubscriptionsAsync(ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, "Invalid user authentication.", null);
            }

            var subscriptions = await _context.Subscriptions
                .Include(x => x.ParkingSpace)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new SubscriptionResponse
                {
                    Id = x.Id,
                    ParkingSpaceId = x.ParkingSpaceId,
                    SpaceNumber = x.ParkingSpace.SpaceNumber,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Amount = x.Amount,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return (true, null, subscriptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while retrieving the user's subscriptions.",
                ex);
        }
    }

    public async Task<(bool Success, string? Error, SubscriptionResponse? Subscription)>
    CancelSubscriptionAsync(
        Guid subscriptionId,
        ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, "Invalid user authentication.", null);
            }

            var subscription = await _context.Subscriptions
                .Include(x => x.ParkingSpace)
                .FirstOrDefaultAsync(x =>
                    x.Id == subscriptionId &&
                    x.UserId == userId);

            if (subscription == null)
            {
                return (false, "Subscription not found.", null);
            }

            if (subscription.Status != SubscriptionStatus.Active)
            {
                return (
                    false,
                    "Only an active subscription can be cancelled.",
                    null);
            }

            subscription.Status = SubscriptionStatus.Cancelled;

            await _context.SaveChangesAsync();

            var response = new SubscriptionResponse
            {
                Id = subscription.Id,
                ParkingSpaceId = subscription.ParkingSpaceId,
                SpaceNumber = subscription.ParkingSpace.SpaceNumber,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                Amount = subscription.Amount,
                Status = subscription.Status.ToString(),
                CreatedAt = subscription.CreatedAt
            };

            return (true, null, response);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while cancelling the subscription.",
                ex);
        }
    }
}