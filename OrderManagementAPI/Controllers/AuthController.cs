using Microsoft.AspNetCore.Mvc;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.Interfaces;

namespace OrderManagementAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterCustomerDto registerCustomerDto,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.Register(registerCustomerDto, cancellationToken));
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromQuery]string email, [FromQuery]string password,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _authService.Login(email, password, cancellationToken));
    }
}