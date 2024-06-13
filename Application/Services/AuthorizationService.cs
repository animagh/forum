using Contracts;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{

    private readonly RoleManager<Role> _roleManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;



    public AuthorizationService(RoleManager<Role> roleManager,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository)
    {

        _roleManager = roleManager;
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto> Authenticate(Expression<Func<User, bool>> expression)
    {
        var user = await _userRepository.GetUser(expression);

        var token = await _tokenGenerator.GenerateToken(user);

        return new LoginResponseDto(user.Id,user.UserName, token);
    }

    public async Task<IdentityResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            Name = registerDto.Name,
            Surname = registerDto.Surname
        };

        var result = await _userRepository.CreateUser(user, registerDto.Password);

        if (!result.Succeeded)
            return result;
        try
        {
            var roleExists = await _roleManager.RoleExistsAsync("User");

            if (roleExists)
                return await _userRepository.AddToRole(user, "User");
            else
                return IdentityResult.Failed(new IdentityError { Code = "Exception", Description = "Role don't Exists" });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Code = "Exception", Description = ex.Message });
        }

    }

    public async Task<IdentityResult> Login(LoginDto loginDto)
    {

        var user = await _userRepository.GetUser(user => user.UserName == loginDto.Username);

        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserDoesNotExist", Description = "The user does not exists." });

        var PasswordCheck = await _userRepository.CheckPassword(user, loginDto.Password);
        if (!PasswordCheck)
            return IdentityResult.Failed(new IdentityError { Code = "IncorrectPassword", Description = "Incorrect password." });

        return IdentityResult.Success;

    }

    public async Task<IdentityResult> AddNewRole(string newRole)
    {
        var roleExists = await _roleManager.RoleExistsAsync("User");

        if (roleExists)
            return IdentityResult.Failed(new IdentityError { Code = "AlreadyExists",Description = "Role Already Exists" });
        Role role = new Role()
        {
            Name = newRole,
            NormalizedName = newRole.ToUpper(),
        };
        return await _roleManager.CreateAsync(role);
            
    }
}