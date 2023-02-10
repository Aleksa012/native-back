using System.ComponentModel.DataAnnotations;

namespace NativeBackendApp.Models;

public class Comment
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime EditedAt { get; set; }

    [Required]
    [MaxLength(150)]
    public string Content { get; set; } = string.Empty;

    public List<Guid> Likes { get; set; } = new List<Guid>();
    
    public List<Guid> Dislikes { get; set; } = new List<Guid>();

    public List<CommentReply> CommentReplies { get; set; } = new List<CommentReply>();
    
    [Required]
    public User Author { get; set; }
}