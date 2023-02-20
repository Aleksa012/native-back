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
public class PostsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly GetUserFromToken _getUserFromToken;
    
    public PostsController(DataContext context ,GetUserFromToken getUserFromToken)
    {
        _context = context;
        _getUserFromToken = getUserFromToken;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostResponse>> GetAll()
    {
        return Ok(_context.Posts.Include(post => post.Author).Include(post => post.Comments).OrderBy(post=> post.CreatedAt).Select(post => new PostResponse(post)));
    }

    [HttpGet]
    [Route("user")]
    public ActionResult<IEnumerable<PostResponse>> GetPostsByUser(Guid? id)
    {
        if (id != null)
        {
            return Ok(_context.Posts.Include(post => post.Author).Include(post => post.Comments).Where(post => post.Author!.Id == id).Select(post => new PostResponse(post)));;
        }
        else
        {
            var user = _getUserFromToken.GetUser(HttpContext);
            
            return Ok(_context.Posts.Include(post => post.Author).Include(post => post.Comments).Where(post => post.Author!.Id == user.Id).Select(post => new PostResponse(post)));;
        }
    }

    [HttpPost]
    public async Task<ActionResult<PostResponse>> CreatePost(PostDto postDto)
    {
        var token = HttpContext.User;
        
        var author = _context.Users.Single(user => user.Id.ToString() == token.FindFirst("Id")!.Value);

        var newPost = new Post(author);

        newPost.Content = postDto.Content;
        newPost.CreatedAt = DateTime.Now.ToUniversalTime();

        await _context.Posts.AddAsync(newPost);

        await _context.SaveChangesAsync();
        
        return Ok(new PostResponse(newPost));
    }

    [HttpPut]
    [Route("like")]
    public async Task<ActionResult<PostResponse>> Like(Guid id)
    {
        var userId = _getUserFromToken.GetUser(HttpContext).Id;
        
        var post = _context.Posts.Include(post => post.Author).Single(post => post.Id == id);

        if (post.Likes.Contains(userId))
        {
            post.Likes.Remove(userId);
        }
        else
        {
            post.Likes.Add(userId);
        }

        if (post.Dislikes.Count + post.Likes.Count >= 10)
        {
            post.IsPopular = true;
        }

        await _context.SaveChangesAsync();

        return Ok(new PostResponse(post));
    }
    
    [HttpPut]
    [Route("dislike")]
    public async Task<ActionResult<PostResponse>> Dislike(Guid id)
    {
        var userId = _getUserFromToken.GetUser(HttpContext).Id;

        var post = _context.Posts.Include(post => post.Author).Single(post => post.Id == id);

        if (post.Dislikes.Contains(userId))
        {
            post.Dislikes.Remove(userId);
        }
        else
        {
            post.Dislikes.Add(userId);
        }
        
        if (post.Dislikes.Count + post.Likes.Count >= 10)
        {
            post.IsPopular = true;
        }

        await _context.SaveChangesAsync();

        return Ok(new PostResponse(post));
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePost(Guid id)
    {
        _context.Posts.Remove(_context.Posts.Find(id)!);

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet]
    [Route("Id")]
    public ActionResult<PostResponse> GetPostById(Guid id)
    {
        var post = _context.Posts.Include(post => post.Author).Include(post => post.Comments).Single(post => post.Id == id);
        
        return Ok(new PostResponse(post));
    }
}