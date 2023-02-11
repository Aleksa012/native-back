using System.ComponentModel.DataAnnotations;

namespace NativeBackendApp.DTO;

public class UserDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    public string Password { get; set; } = string.Empty;
    
    public string? PasswordConfirmation { get; set; }
}