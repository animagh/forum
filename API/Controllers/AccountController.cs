using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    public AccountController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody]RegisterDto registerDto)
    {
        var registrationCheckUp = await _authorizationService.Register(registerDto);

        if (!registrationCheckUp.Succeeded)
            return BadRequest(registrationCheckUp.Errors);

        return Ok("Registration successful");
    }


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var loginCheckUp = await _authorizationService.Login(loginDto);

        if (!loginCheckUp.Succeeded)
            return BadRequest(loginCheckUp.Errors);

        return Ok(await _authorizationService.Authenticate(user => user.UserName == loginDto.Username));
    }

}
