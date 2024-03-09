using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VendingMachine.Api.Contracts;
using VendingMachine.Api.Infrastructer;
using VendingMachine.Applicaion.Products.Commands.CreateProduct;
using VendingMachine.Applicaion.Products.Commands.DeleteProduct;
using VendingMachine.Applicaion.Products.Commands.UpdateProduct;
using VendingMachine.Applicaion.Products.Commands.UpdateProductPrice;
using VendingMachine.Applicaion.Products.Queries.GetProductInfo;
using VendingMachine.Applicaion.Products.Queries.GetProductsQuery;
using VendingMachine.Applicaion.Sellers.Command.AddProductAmount;
using VendingMachine.Domain;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Api.Controllers;

public class ProductController : ApiController
{
  
    public ProductController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
    {
         
    }

    [Authorize(Roles = "Seller")]
    [HttpPost("product/create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
  
        var user = GetCurrentUser();

        if (request == null)
            return BadRequest($"{nameof(CreateProductRequest)} was null");

  
         
        Result<Product> commandResult = await Mediator.Send(new CreateProductCommand( request.Name,request.price, user.UserId,request.Descreption));
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult.Value);
    }

    [HttpGet("product/{productId:guid}")]
    [ProducesResponseType(typeof(ProductInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid productId)
    { 

        Result<ProductInfo> commandResult = await Mediator.Send(new GetProductInfoQuery(productId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound();
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);

 

        return Ok(commandResult.Value);
    }

   
    [HttpGet("product")]
    [ProducesResponseType(typeof(List<ProductInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
      
        var commandResult = await Mediator.Send(new GetProductsQuery());
   
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors); 
     
        return Ok(commandResult.Value);
    }

    [Authorize(Roles = "Seller")]
    [HttpPut("product/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid productId, [FromBody] UpdateProductCommand request)
    {
        if (request == null)
            return BadRequest($"{nameof(UpdateProductCommand)} was null");


        var user = GetCurrentUser();

        Result<ProductInfo> productResult = await Mediator.Send(new GetProductInfoQuery(productId));
        if (productResult.IsFailure )
            return BadRequest(productResult.Errors);

        if (user.UserId != productResult.Value.SellerId)
            return Forbid();




        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound();
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }


    [Authorize(Roles = "Seller")]
    [HttpPut("product/setPrice/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePrice(Guid productId, [FromBody] UpdateProductPriceCommand request)
    {

        if (request == null)
            return BadRequest($"{nameof(UpdateProductPriceCommand)} was null");

        var user = GetCurrentUser();

        Result<ProductInfo> productResult = await Mediator.Send(new GetProductInfoQuery(productId));
        if (productResult.IsFailure)
            return BadRequest(productResult.Errors);

        if (user.UserId != productResult.Value.SellerId)
            return Forbid();



        Result commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound();
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();
    }

    [Authorize(Roles = "Seller")]
    [HttpPut("product/addAmount/{productId:guid}")]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProductAmount(Guid productId, [FromBody] AddProductAmountCommand request)
    {

        if (request == null)
            return BadRequest($"{nameof(UpdateProductPriceCommand)} was null");

        var user = GetCurrentUser();

        Result<ProductInfo> productResult = await Mediator.Send(new GetProductInfoQuery(productId));
        if (productResult.IsFailure)
            return BadRequest(productResult.Errors);

        if (user.UserId != productResult.Value.SellerId)
            return Forbid();



        Result<int> commandResult = await Mediator.Send(request);
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound();
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok(commandResult);
    }


    [Authorize(Roles = "Seller")]
    [HttpDelete("product/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid productId)
    {


         

        var user = GetCurrentUser();

        Result<ProductInfo> productResult = await Mediator.Send(new GetProductInfoQuery(productId));
        if (productResult.IsFailure)
            return BadRequest(productResult.Errors);

        if (user.UserId != productResult.Value.SellerId)
            return Forbid();

        Result commandResult = await Mediator.Send(new DeleteProductCommand(productId));
        if (commandResult.IsFailure && commandResult.Errors.Any(e => e.Code == nameof(EntityNotFoundException)))
            return NotFound();
        if (commandResult.IsFailure)
            return BadRequest(commandResult.Errors);
        return Ok();

    }

}

public record CreateProductRequest(string Name , string? Descreption ,int price);
