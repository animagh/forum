using AutoMapper;
using Contracts;
using Domain.Entities;
using Domain.Models;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;

    public CommentService(ITopicRepository topicRepository, ICommentRepository commentRepository, IMapper mapper)
    {
        _topicRepository = topicRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
    }
    public CommentService()
    {
        
    }
    public async Task CreateComment(CreateCommentDto commentDto)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(commentDto.TopicId);
        if (topic == null)
            throw  new NotFoundException("Topic Not Found");
        if (topic.Status == Status.Inactive)
            throw new RestrictedException("You cant comment on this post");
        var comment = _mapper.Map<Comment>(commentDto);
        comment.UserId = commentDto.userId;

        _commentRepository.AddCommentAsync(comment);
        await _commentRepository.SaveAsync();
    }
    public async Task UpdateComment(int commentId,UpdateCommentDto commentDto)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null)
            throw new NotFoundException("Comment Not Found");


        comment.Body = commentDto.Body;


        _commentRepository.UpdateCommentAsync(comment);
        await _commentRepository.SaveAsync();
    }
    public async Task DeleteComment(int commentId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null)
            throw new NotFoundException("Comment Not Found");


        _commentRepository.DeleteCommentAsync(comment);
        await _commentRepository.SaveAsync();
    }
}
