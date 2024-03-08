using MediatR;
using Microsoft.AspNetCore.Authorization;
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
 
    public BuyerController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
    {
        
    }


    [Authorize(Roles = "Buyer")]
    [HttpGet("buyer/{buyerId:guid}")]
    [ProducesResponseType(typeof(Buyer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid buyerId)
    {
        var user = GetCurrentUser();
        if (user.Type != Identity.UserType.Buyer || user.UserId != buyerId)
            return Forbid();

        Result<Buyer> commandResult = await Mediator.Send(new GetBuyerInfoQuery(buyerId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }


    [Authorize(Roles = "Buyer")]
    [HttpPut("buyer/{buyerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid buyerId, [FromBody] UpdateBuyerCommand request)
    {

        if (request == null)
            return BadRequest($"{nameof(UpdateBuyerCommand)} was null");

        var user = GetCurrentUser();
        if (user.Type != Identity.UserType.Buyer || user.UserId != buyerId)
            return Forbid();


      

        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }



}
