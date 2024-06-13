using Contracts;
using Domain.Entities;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class TopicRepository: ITopicRepository
{
    private readonly ApplicationDataContext _context;

    public TopicRepository(ApplicationDataContext context)
    {
        _context = context;
    }
    public async Task AddTopictAsync(Topic topic) => _context.Topics.Add(topic);

    public async Task DeleteTopicAsync(Topic topic) => _context.Topics.Remove(topic);

    public async Task UpdateTopicAsync(Topic topic) => _context.Topics.Update(topic);

    public async Task<IEnumerable<Topic>> GetAllTopicAsync() => await _context.Topics.Include(t=>t.User).ToListAsync();
    public async Task<IEnumerable<Topic>> GetAllTopicWithContentAsync() =>
    await _context.Topics
        .Include(t=>t.Comments)
            .ThenInclude(c=>c.User)
        .Include(t=>t.User)
        .ToArrayAsync();

    public async Task<Topic> GetTopicByIdAsync(int id) => 
        await _context.Topics.Where(t => t.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Topic>> GetTopicByUserIdAsync(string userId) => 
        await _context.Topics.Where(t=>t.UserId == userId).ToListAsync();

    public async Task<IEnumerable<Topic>> GetAllTopicAsyncWithConditionAsync(Expression<Func<Topic, bool>> expression) => 
        await _context.Topics.Where(expression).ToListAsync();

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
