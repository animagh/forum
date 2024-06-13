using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommentController:ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody]CreateCommentDto commentDto)
    {

        await _commentService.CreateComment(commentDto);

        return Ok("Comment Added Succesfully");

    }
    [Authorize(Roles = "User")]
    [HttpPut("update-comment/{commentId}")]
    public async Task<IActionResult> UpdateComment([FromBody]UpdateCommentDto commentDto,int commentId)
    {

        await _commentService.UpdateComment(commentId, commentDto);

        return Ok("Comment updated Succesfully");

    }
    [Authorize(Roles = "User")]
    [HttpDelete("delete-comment/{userId}/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {

        await _commentService.DeleteComment(commentId);

        return Ok("Comment deleted Succesfully");

    }
}
