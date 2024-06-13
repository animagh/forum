using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Comment body must be between 1 and 500 characters.")]
    public string Body { get; set; }

    [Required]
    public string UserId { get; set; }

    public User User { get; set; }

    [Required]
    public int TopicId { get; set; }

}