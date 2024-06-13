using Domain.Entities;
using Domain.Models;
using System.Security.Claims;

namespace Contracts;

public interface IUserService
{
    Task<UserDto> GetUserWithEmail(string email);
    Task<IEnumerable<UserDto>> GetUsers();
    Task UserBanStatusChange(string userId, Ban ban);
} 
