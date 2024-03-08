using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Api.Contracts;
using VendingMachine.Api.Infrastructer;
using VendingMachine.Applicaion.Buyers.Command.CreateBuyer;
using VendingMachine.Applicaion.Buyers.Command.UpdateBuyer;
using VendingMachine.Applicaion.Buyers.Queries.GetBuyerInfo;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Api.Controllers;

public class BuyerController: ApiController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BuyerController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _httpContextAccessor = httpContextAccessor;
    }

   

    [HttpGet("buyer/{buyerId:guid}")]
    [ProducesResponseType(typeof(Buyer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid buyerId)
    {
        //var currentBuyerId = GetCurrentBuyerId();
        //if (buyerId != currentBuyerId)
        //    return Forbid();

        Result<Buyer> commandResult = await Mediator.Send(new GetBuyerInfoQuery(buyerId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }

  

    [HttpPut("buyer/{buyerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid buyerId, [FromBody] UpdateBuyerCommand request)
    {
        var currentBuyerId = GetCurrentBuyerId();
        if (buyerId != currentBuyerId)
            return Forbid();

        if (request == null)
            return BadRequest($"{nameof(UpdateBuyerCommand)} was null");

      

        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }

    private Guid GetCurrentBuyerId()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        return Guid.Parse(userId);
    }

}
