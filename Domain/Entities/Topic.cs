using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Topic
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public string Title { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Body must be between 1 and 1000 characters.")]
    public string Body { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public int CommentNum => Comments?.Count ?? 0;

    [EnumDataType(typeof(State))]
    public State State { get; set; } = State.Pending;

    [EnumDataType(typeof(Status))]
    public Status Status { get; set; } = Status.Active;

    public ICollection<Comment> Comments { get; set; }

    [Required]
    public string UserId { get; set; }

    public User User { get; set; }
}