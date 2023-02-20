using System.ComponentModel.DataAnnotations;
using NativeBackendApp.Models;

namespace NativeBackendApp.ResponseModels;

public class CommentResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? EditedAt { get; set; }

    [Required]
    public string Content { get; set; }
    
    public List<Guid> Likes { get; set; }
    
    public List<Guid> Dislikes { get; set; } 
    
    public string Author { get; set; }

    public CommentResponse(Comment comment)
    {
        Id = comment.Id;
        CreatedAt = DateTime.Now.ToUniversalTime();
        EditedAt = comment.EditedAt;
        Content = comment.Content;
        Likes = comment.Likes;
        Dislikes = comment.Dislikes;
        Author = comment.Author.UserName;
    }
}