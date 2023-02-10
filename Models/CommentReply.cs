using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NativeBackendApp.Models;

public class CommentReply
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public User Author { get; set; }
}