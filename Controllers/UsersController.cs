using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NativeBackend.Services;
using NativeBackendApp.Data;
using NativeBackendApp.DTO;
using NativeBackendApp.Models;
using NativeBackendApp.ResponseModels;
using NativeBackendApp.Util;
using Microsoft.EntityFrameworkCore;

namespace NativeBackendApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly DataContext _context;
    private readonly Auth _auth;
    
    public UsersController(DataContext context , Auth auth)
    {
        _context = context;
        _auth = auth;
    }
    
    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<UserResponse>> GetAll()
    {
        return Ok(_context.Users.Select(user =>
                new UserResponse(user , _context.Posts.Where(post => post.Author.Id == user.Id).ToList())       
        ));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> PostUser(UserDto userDto)
    {
        if (_context.Users.Any<User>(user => user.UserName == userDto.UserName))
        {
            return BadRequest();
        }

        if (userDto.PasswordConfirmation != userDto.Password)
        {
            return BadRequest();
        }

        User user = new User();

        if (userDto.Email != null)
        {
            user.IsVerified = true;
        }

        byte[] passwordHash;
        byte[] passwordSalt;

        PasswordHasher.HashUserPassword(userDto.Password, out passwordHash, out passwordSalt);

        user.Email = userDto.Email;
        user.UserName = userDto.UserName;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _context.AddAsync<User>(user);

        await _context.SaveChangesAsync();

        return Ok(new UserResponse(user));
    }

    [HttpPost]
    [Route("login")]
    public ActionResult<string> Login(UserDto userDto)
    {
        
        if (!_context.Users.Any<User>(user => user.UserName == userDto.UserName))
        {
            return BadRequest("Does not exist");
        }

        var logedInUser = _context.Users.Single<User>(user => user.UserName == userDto.UserName);

        if (!PasswordHasher.VerifyHash(userDto.Password, logedInUser.PasswordHash, logedInUser.PasswordSalt))
        {
            return BadRequest("Wrong credentials");
        }

        return Ok(_auth.GetToken(logedInUser));
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var token = HttpContext.User;
        
        var user = _context.Users.Single(user => user.Id.ToString() == token.FindFirst("Id")!.Value);

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return Ok();
    }
}