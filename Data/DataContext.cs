using Microsoft.EntityFrameworkCore;
using NativeBackendApp.Models;

namespace NativeBackendApp.Data;

public class DataContext : DbContext
{
#pragma warning disable CS8618
    public DataContext(DbContextOptions<DataContext> options)
#pragma warning restore CS8618
        :base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentReply> CommentReplies { get; set; }
}