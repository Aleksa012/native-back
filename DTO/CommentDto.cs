using System.ComponentModel.DataAnnotations;

namespace NativeBackendApp.DTO;

public class CommentDto
{
    [Required]
    [MaxLength(200)]
    public string Content { get; set; } = string.Empty;
}