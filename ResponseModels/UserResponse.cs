using NativeBackendApp.Models;

namespace NativeBackendApp.ResponseModels;

[Serializable]
public class UserResponse
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string? Email { get; set; }

    public bool IsVerified { get; set; }

    public List<PostResponse> Posts { get; set; } = new List<PostResponse>();
    
    public UserResponse(User user )
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
        IsVerified = user.IsVerified;
    }
    
    public UserResponse(User user , List<Post> posts)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
        IsVerified = user.IsVerified;
        Posts = posts.Select(post => new PostResponse(post)).ToList();
    }
}