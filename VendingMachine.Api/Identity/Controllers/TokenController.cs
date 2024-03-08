using VendingMachine.Api.Identity.TokenHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VendingMachine.Api.Identity.Controllers;


[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public TokenController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest tokenDto)
    {
        if (tokenDto is null)
        {
            return BadRequest(new AuthResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
        }

        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);
        var username = principal.Identity.Name;
        var userId = principal.FindFirstValue(ClaimTypes.Actor);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest(new AuthResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

        var signingCredentials = _tokenService.GetSigningCredentials();
        var claims = await _tokenService.GetClaims(user);
        var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        user.RefreshToken = _tokenService.GenerateRefreshToken();

        await _userManager.UpdateAsync(user);

        return Ok(new AuthResponse { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
    }
}