using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NativeBackend.Services;
using NativeBackendApp.Data;
using NativeBackendApp.DTO;
using NativeBackendApp.Models;
using NativeBackendApp.ResponseModels;

namespace NativeBackendApp.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly DataContext _context;

    private readonly GetUserFromToken _getUserFromToken;
    
    public CommentsController(DataContext context , GetUserFromToken currentUser)
    {
        _context = context;
        _getUserFromToken = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<CommentResponse>> PostComment(CommentDto commentDto , Guid postId)
    {
        var user = _getUserFromToken.GetUser(HttpContext);
        
        var post = _context.Posts.Include(post => post.Author).Single(post => post.Id == postId);
        
        var newComment = new Comment();

        newComment.Author = user;
        newComment.Content = commentDto.Content;
        newComment.CreatedAt = DateTime.Now.ToUniversalTime();
        newComment.EditedAt = DateTime.Now.ToUniversalTime();
        
        post.Comments.Add(newComment);


        await _context.Comments.AddAsync(newComment);

        _context.SaveChanges();

        return Ok(new CommentResponse(newComment));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteComment(Guid id , Guid postId)
    {
        var post = _context.Posts.Include(post => post.Author).Single(post => post.Id == postId);
        
        post.Comments.Remove(_context.Comments.Find(id)!);
        
        _context.Comments.Remove(_context.Comments.Find(id)!);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPut]
    [Route("like")]
    public async Task<ActionResult> Like(Guid id , Guid userId)
    {
        var comment = _context.Comments.Include(comment => comment.Author).Single(comment => comment.Id == id);

        if (comment.Likes.Contains(userId))
        {
            comment.Likes.Remove(userId);
        }
        else
        {
            comment.Likes.Add(userId);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPut]
    [Route("dislike")]
    public async Task<ActionResult> Dislike(Guid id , Guid userId)
    {
        var comment = _context.Comments.Include(comment => comment.Author).Single(comment => comment.Id == id);

        if (comment.Dislikes.Contains(userId))
        {
            comment.Dislikes.Remove(userId);
        }
        else
        {
            comment.Dislikes.Add(userId);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }
}