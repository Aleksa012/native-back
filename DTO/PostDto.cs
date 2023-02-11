using System.ComponentModel.DataAnnotations;

namespace NativeBackendApp.DTO;

public class PostDto
{
    [Required]
    [MaxLength(200)]
    public string Content { get; set; } = String.Empty;
    
}