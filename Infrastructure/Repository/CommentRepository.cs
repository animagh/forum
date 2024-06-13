using Contracts;
using Domain.Entities;
using Infrastructure.DataConnection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class CommentRepository: ICommentRepository
{
    private readonly ApplicationDataContext _context;

    public CommentRepository(ApplicationDataContext context)
    {
        _context = context;
    }
    public async Task AddCommentAsync(Comment comment) => _context.Comments.Add(comment);

    public async Task DeleteCommentAsync(Comment comment) => _context?.Comments.Remove(comment);
    public async Task UpdateCommentAsync(Comment comment) => _context?.Comments.Update(comment);

    public async Task<Comment> GetCommentByIdAsync(int id) => 
        await _context.Comments.Where(c => c.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Comment>> GetCommentsByTopicIdAsync(int topicId) => 
        await _context.Comments.Where(c => c.TopicId == topicId).ToListAsync();

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId) => 
        await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
    public async Task<IEnumerable<Comment>> GetCommentsAsync() => await _context.Comments.ToListAsync();

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
