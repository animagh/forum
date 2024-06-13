using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicController:ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }
    [HttpGet]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await _topicService.GetTopics();

        return Ok(topics);
    }
    [Authorize(Roles = "User")]
    [HttpGet("with-content")]
    public async Task<IActionResult> GetTopicsWithContent()
    {
        var topics = await _topicService.GetTopicsWithContent();

        return Ok(topics);
    }

    [Authorize(Roles = "User")]
    [HttpPost("add-topic/{userId}")]
    public async Task<IActionResult> AddTopic(string userId, [FromBody] CreateTopicDto createTopicDto)
    {
        await _topicService.CreateTopic(userId, createTopicDto);

        return Ok("Topic add Successfully");
    }
    [Authorize(Roles = "User")]
    [HttpPut("update-topic/{topicId}")]
    public async Task<IActionResult> UpdateTopic([FromBody] UpdateTopicDto updateTopicDto, int topicId)
    {
        
        await _topicService.UpdateTopic(topicId, updateTopicDto);

        return Ok("Topic Updated Successfully");
    }
    [Authorize(Roles = "User")]
    [HttpDelete("delete-topic/{topicId}")]
    public async Task<IActionResult> DeleteTopic(int topicId)
    {
        await _topicService.DeleteTopic( topicId);

        return Ok("Topic Deleted Successfully");
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("change-topic-state/{topicId}/{state}")]
    public async Task<IActionResult> ChangeTopicState(int topicId, State state)
    {
        await _topicService.ChangeTopicState(topicId, state);
        return Ok($"Topic state updated to: {state.ToString()}");
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("change-topic-status/{topicId}/{status}")]
    public async Task<IActionResult> ChangeTopicStatus(int topicId, Status status) 
    {
        await _topicService.ChangeTopicStatus(topicId, status);
        return Ok($"Topic status updated to: {status.ToString()}");
    }

}
