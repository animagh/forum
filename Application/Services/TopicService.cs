using AutoMapper;
using Contracts;
using Domain.Entities;
using Domain.Models;

namespace Application.Services;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;

    public TopicService(ITopicRepository topicRepository, IMapper mapper)
    {
        _topicRepository = topicRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TopicDto>> GetTopics()
    {
        var topics = await _topicRepository.GetAllTopicAsync();

        if (!topics.Any())
            throw new NotFoundException("Topics Not Found");


        return _mapper.Map<IEnumerable<TopicDto>>(topics);
    }

    public async Task<IEnumerable<TopicWithContentDto>> GetTopicsWithContent()
    {
        var topics = await _topicRepository.GetAllTopicWithContentAsync();

        if (!topics.Any())
            throw new NotFoundException("Topics Not Found");


        return _mapper.Map<IEnumerable<TopicWithContentDto>>(topics);
    }

    public async Task CreateTopic(string userId,CreateTopicDto createTopicDto)
    {
        var topic = _mapper.Map<Topic>(createTopicDto);
        topic.UserId = userId;
        _topicRepository.AddTopictAsync(topic);

        await _topicRepository.SaveAsync();
    }

    public async Task UpdateTopic(int topicId,UpdateTopicDto createTopicDto)
    {
       
        var topic = await _topicRepository.GetTopicByIdAsync(topicId);
        if (topic.Status == Status.Inactive)
            throw new RestrictedException("You cant comment on this post");
        if (topic == null)
            throw new NotFoundException("Topic not found");

        topic.Body = createTopicDto.Body;
        topic.Title = createTopicDto.Title;

        _topicRepository.UpdateTopicAsync(topic);

        await _topicRepository.SaveAsync();
    }

    public async Task DeleteTopic(int topicId)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(topicId);
        if (topic.Status == Status.Inactive)
            throw new RestrictedException("You cant comment on this post");

        if (topic == null)
            throw new NotFoundException("Topic not found");

        _topicRepository.DeleteTopicAsync(topic);

        await _topicRepository.SaveAsync();
    }
    public async Task ChangeTopicState(int topicId, State state)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(topicId);
        if (topic == null)
            throw new NotFoundException("Topic not found");
        if (state == State.Pending)
            throw new InvalidArgumentException("Can't Set this state");

        topic.State = state;
        _topicRepository.UpdateTopicAsync(topic);

        await _topicRepository.SaveAsync();
    }
    public async Task ChangeTopicStatus(int topicId,Status status)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(topicId);
        if (topic == null)
            throw new NotFoundException("Topic not found");

        topic.Status = status;


        _topicRepository.UpdateTopicAsync(topic);

        await _topicRepository.SaveAsync();
    }


}
