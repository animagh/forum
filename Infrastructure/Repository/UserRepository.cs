using Contracts;
using Domain.Entities;
using Infrastructure.DataConnection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDataContext _context;

    public UserRepository(UserManager<User> userManager, ApplicationDataContext context)
    {
        _userManager = userManager;
        _context = context; 
    }

    public async Task<IdentityResult> AddToRole(User user, string roles) => await _userManager.AddToRoleAsync(user, roles);

    public async Task<bool> CheckPassword(User user, string password) => await _userManager.CheckPasswordAsync(user, password);

    public async Task<IdentityResult> CreateUser(User user, string passord) => await _userManager.CreateAsync(user, passord);

    public async Task<IEnumerable<User>> GetAllUsers() => await _userManager.Users.ToListAsync();
    public async Task<IEnumerable<User>> GetUsersWithCondition(Expression<Func<User, bool>> expression) => await _userManager.Users.Where(expression).ToListAsync();

    public async Task<User> GetUser(Expression<Func<User, bool>> expression) => await _userManager.Users.FirstOrDefaultAsync(expression);

    public async Task<User> GetUserFromClaim(ClaimsPrincipal claimsPrincipal) => await _userManager.GetUserAsync(claimsPrincipal);

    public async Task UpdateUser(User user) =>  await _userManager.UpdateAsync(user);

    public async Task<IdentityResult> ConfirmEmailI(User user, string token) => await _userManager.ConfirmEmailAsync(user, token);


}
