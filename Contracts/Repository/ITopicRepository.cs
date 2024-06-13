using Domain.Entities;
using System.Linq.Expressions;

namespace Contracts;

public interface ITopicRepository : ISaveAble
{
    Task AddTopictAsync(Topic topic);
    Task DeleteTopicAsync(Topic topic);
    Task UpdateTopicAsync(Topic topic);
    Task<Topic> GetTopicByIdAsync(int id);
    Task<IEnumerable<Topic>> GetTopicByUserIdAsync(string userId);
    Task<IEnumerable<Topic>> GetAllTopicAsync();
    Task<IEnumerable<Topic>> GetAllTopicAsyncWithConditionAsync(Expression<Func<Topic, bool>> expression);
    Task<IEnumerable<Topic>> GetAllTopicWithContentAsync();
}
