using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController:ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsers(); 
        return Ok(users);
    }
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserWithMail(string email)
    {
        var user = await _userService.GetUserWithEmail(email);
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}/{ban}")]
    public async Task<IActionResult> BanUser(string userId ,Ban ban)
    {
        await _userService.UserBanStatusChange(userId, ban);
        return Ok($"User {ban} Successfully");
    }


}
