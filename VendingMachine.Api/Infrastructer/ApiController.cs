using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Api.Contracts;
using VendingMachine.Api.Identity;
using VendingMachine.Domain.Core;

namespace VendingMachine.Api.Infrastructer;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api")]
public class ApiController : ControllerBase
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMediator Mediator;

    protected ApiController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        Mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }


    protected IActionResult BadRequest(params Error[] error) => BadRequest(new ApiErrorResponse(error));
    protected new IActionResult Ok(object value) => base.Ok(value);

    protected new IActionResult NotFound() => base.NotFound();

    internal (Guid UserId,UserType Type)GetCurrentUser()
    {

        var user = _httpContextAccessor.HttpContext.User;
        var userId =user.FindFirstValue(ClaimTypes.Actor);
        var actorType = user.FindFirstValue("ActorType");
        UserType type = (UserType)int.Parse(actorType);
        
            return (Guid.Parse(userId),type);  
    }
}
