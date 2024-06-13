using Domain.Models;

namespace Contracts;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetTopics();
    Task<IEnumerable<TopicWithContentDto>> GetTopicsWithContent();
    Task CreateTopic(string userId, CreateTopicDto createTopicDto);
    Task UpdateTopic(int topicId, UpdateTopicDto updateTopicDto);
    Task DeleteTopic(int topicId);
    Task ChangeTopicState(int topicId, State state);
    Task ChangeTopicStatus( int topicId, Status status);
}
