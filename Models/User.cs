namespace NativeBackendApp.Models;

public class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public string? Email { get; set; }

    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public bool IsVerified { get; set; } = false;

    public List<Post> Posts { get; set; } = new List<Post>();

    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<CommentReply> CommentReplies { get; set; } = new List<CommentReply>();
}