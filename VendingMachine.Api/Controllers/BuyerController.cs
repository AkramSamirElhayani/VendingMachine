using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Api.Contracts;
using VendingMachine.Api.Infrastructer;
using VendingMachine.Applicaion.Buyers.Command.Buy;
using VendingMachine.Applicaion.Buyers.Command.CreateBuyer;
using VendingMachine.Applicaion.Buyers.Command.Deposit;
using VendingMachine.Applicaion.Buyers.Command.UpdateBuyer;
using VendingMachine.Applicaion.Buyers.Command.Withdraw;
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
    [HttpGet("buyer")]
    [ProducesResponseType(typeof(Buyer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var user = GetCurrentUser();
         

        Result<Buyer> commandResult = await Mediator.Send(new GetBuyerInfoQuery(user.UserId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }


    [Authorize(Roles = "Buyer")]
    [HttpPut("buyer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update( [FromBody] UpdateBuyerCommand request)
    {

        if (request == null)
            return BadRequest($"{nameof(UpdateBuyerCommand)} was null");

        var user = GetCurrentUser();
        if (user.Type != Identity.UserType.Buyer || user.UserId != request.BuyerId)
            return Forbid();


      

        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("buyer/deposit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deposit( [FromBody] DepositCommand request)
    {
        

        if (request == null)
            return BadRequest($"{nameof(DepositCommand)} was null");

        var user = GetCurrentUser();
        if (user.UserId != request.BuyerId)
            return Forbid();
         

        Result<int> commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }

    [Authorize(Roles = "Buyer")]
    [HttpPut("buyer/withdraw")]
    [ProducesResponseType(typeof(Dictionary<int,int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand request)
    {


        if (request == null)
            return BadRequest($"{nameof(WithdrawCommand)} was null");

        var user = GetCurrentUser();
        if (user.UserId != request.BuyerId)
            return Forbid();


        Result<Dictionary<int,int>> commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("buyer/buy")]
    [ProducesResponseType(typeof(BuyCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Buy([FromBody] BuyCommand request)
    {


        if (request == null)
            return BadRequest($"{nameof(WithdrawCommand)} was null");

        var user = GetCurrentUser();
        if (user.UserId != request.BuyerId)
            return Forbid();


        Result<BuyCommandResponse> commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }
}
