using System.ComponentModel.DataAnnotations;

namespace NativeBackendApp.Models;

public class Post
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime EditedAt { get; set; }

    [Required] [MaxLength(200)] public string Content { get; set; } = string.Empty;

    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public List<Guid> Likes { get; set; } = new List<Guid>();
    
    public List<Guid> Dislikes { get; set; } = new List<Guid>();

    public bool IsPopular { get; set; } = false;
    
    [Required]
    public User Author { get; set; }
}