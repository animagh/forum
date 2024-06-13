using AutoMapper;
using Contracts;
using Domain.Entities;
using Domain.Models;
using System.Security.Claims;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _userRepository
            .GetUsersWithCondition(u => u.Roles.Any(r => r.RoleId == "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B"));
        if (!users.Any())
            throw new NotFoundException("Users not found");

        return _mapper.Map<IEnumerable<UserDto>>(users);

    }

    public async Task<UserDto> GetUserWithEmail(string email)
    {
        var user = await _userRepository
            .GetUser(u => u.Email == email && u.Roles.Any(r => r.RoleId == "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B"));
        if (user == null)
            throw new NotFoundException("User On this mail not found");


        return _mapper.Map<UserDto>(user);
    }


    public async Task UserBanStatusChange(string userId, Ban ban)
    {
        var user = await _userRepository.GetUser(u => u.Id == userId);
        if (user == null)
            throw new NotFoundException("Users not found");

        user.Banned = ban;

        await _userRepository.UpdateUser(user);

    }

}
