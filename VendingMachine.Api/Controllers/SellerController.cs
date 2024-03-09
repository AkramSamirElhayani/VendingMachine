﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.Contracts;
using VendingMachine.Api.Infrastructer;
using VendingMachine.Applicaion.Sellers.Command.UpdateSeller;
using VendingMachine.Applicaion.Sellers.Queries.GetSellerInfo;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Api.Controllers;

public class SellerController:ApiController
{
    public SellerController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
    {

    }


    [Authorize(Roles = "Seller")]
    [HttpGet("seller/{sellerId:guid}")]
    [ProducesResponseType(typeof(Seller), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid sellerId)
    {
        var user = GetCurrentUser();
        if (user.Type != Identity.UserType.Seller || user.UserId != sellerId)
            return Forbid();

        Result<Seller> commandResult = await Mediator.Send(new GetSellerInfoQuery(sellerId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }


    [Authorize(Roles = "Seller")]
    [HttpPut("seller/{sellerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid sellerId, [FromBody] UpdateSellerCommand request)
    {

        if (request == null)
            return BadRequest($"{nameof(UpdateSellerCommand)} was null");

        var user = GetCurrentUser();
        if (user.Type != Identity.UserType.Seller || user.UserId != sellerId)
            return Forbid();




        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound(commandResult.Errors);
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }


}
