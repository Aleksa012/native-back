namespace NativeBackendApp.Models;

public class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public string? Email { get; set; }

    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public bool IsVerified { get; set; } = false;

}