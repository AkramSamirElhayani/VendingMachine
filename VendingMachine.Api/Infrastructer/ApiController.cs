using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Api.Contracts;
using VendingMachine.Domain.Core;

namespace VendingMachine.Api.Infrastructer;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api")]
public class ApiController : ControllerBase
{


    protected readonly IMediator Mediator;

    protected ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }


    protected IActionResult BadRequest(params Error[] error) => BadRequest(new ApiErrorResponse(error));
    protected new IActionResult Ok(object value) => base.Ok(value);

    protected new IActionResult NotFound() => base.NotFound();

}
