using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Contracts;

public interface IAuthorizationService
{

    Task<IdentityResult> Login(LoginDto loginDto);
    Task<LoginResponseDto> Authenticate(Expression<Func<User, bool>> expression);
    Task<IdentityResult> Register(RegisterDto registerDto);
    Task<IdentityResult> AddNewRole(string role);


}
