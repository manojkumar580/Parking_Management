using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking_Management.Server.DTOs.Subscription;
using Parking_Management.Server.Services;

namespace Parking_Management.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(
        SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(
    CreateSubscriptionRequest request)
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
                await _subscriptionService.CreateSubscriptionAsync(
                    request,
                    User);

            if (!result.Success)
            {
                if (result.Error ==
                    "Parking space not found or inactive.")
                {
                    return NotFound(new
                    {
                        message = result.Error
                    });
                }

                if (result.Error ==
                    "Parking space is already reserved by an active subscription." ||
                    result.Error ==
                    "Parking space is currently occupied by an active booking.")
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
                result.Subscription);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message =
                    "An unexpected error occurred while creating the subscription."
            });
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMySubscriptions()
    {
        try
        {
            var result =
                await _subscriptionService.GetMySubscriptionsAsync(User);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(result.Subscriptions);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message =
                    "An unexpected error occurred while retrieving subscriptions."
            });
        }
    }

    [HttpPost("{subscriptionId:guid}/cancel")]
    public async Task<IActionResult> CancelSubscription(
    Guid subscriptionId)
    {
        try
        {
            if (subscriptionId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Subscription ID is required."
                });
            }

            var result =
                await _subscriptionService.CancelSubscriptionAsync(
                    subscriptionId,
                    User);

            if (!result.Success)
            {
                if (result.Error == "Subscription not found.")
                {
                    return NotFound(new
                    {
                        message = result.Error
                    });
                }

                if (result.Error ==
                    "Only an active subscription can be cancelled.")
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

            return Ok(result.Subscription);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message =
                    "An unexpected error occurred while cancelling the subscription."
            });
        }
    }
}