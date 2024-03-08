using VendingMachine.Api.Identity.TokenHelpers; 

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendingMachine.Applicaion.Buyers.Command.CreateBuyer;
using VendingMachine.Applicaion.Sellers.Command.CreateSeller;
using VendingMachine.Domain.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendingMachine.Api.Identity.Controllers
{
    public class AccountController : ControllerBase
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;
        public AccountController(
            IMediator mediator,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            Guid actorId; ;
            Result<Guid> actorResult;
            switch (model.UserType)
            {
                case UserType.Buyer:
                    actorResult = await _mediator.Send(new CreateBuyerCommand(model.Username));
                    break;
                case UserType.Seller:
                    actorResult = await _mediator.Send(new CreateSellerCommand(model.Username));
                    break;
                default: throw new NotImplementedException();
            }
            if (actorResult.IsFailure)
                return BadRequest(new RegistrationResponse { Errors =  actorResult.Errors.Select(x=>$"{x.Code}: {x.Message}") });
            else
                actorId = actorResult.Value;


            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                RefreshToken = string.Empty,
                UserType = model.UserType,
                ActorId = actorId
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                
                return Ok(new RegistrationResponse { IsSuccessfulRegistration = true,ActorId = actorId });
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponse { Errors = errors });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new AuthResponse { ErrorMessage = "Invalid Authentication" });


            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);


            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponse { IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken, ActorId = user.ActorId });



        }
    }
}
