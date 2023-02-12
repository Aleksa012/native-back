using System.ComponentModel.DataAnnotations;
using NativeBackendApp.Models;

namespace NativeBackendApp.ResponseModels;

[Serializable]
public class PostResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? EditedAt { get; set; }

    [Required]
    public string Content { get; set; }

    public List<CommentResponse> Comments { get; set; }
    
    public List<Guid> Likes { get; set; }
    
    public List<Guid> Dislikes { get; set; } 

    public bool IsPopular { get; set; }
    
    public Guid Author { get; set; }

    public PostResponse(Post post)
    {
        Id = post.Id;
        CreatedAt = post.CreatedAt;
        EditedAt = post.EditedAt;
        Content = post.Content;
        Comments = post.Comments.Select(comment => new CommentResponse(comment)).ToList();
        Likes = post.Likes;
        Dislikes = post.Dislikes;
        IsPopular = post.IsPopular;
        Author = post.Author!.Id;
    }

}