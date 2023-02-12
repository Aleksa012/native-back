using NativeBackendApp.Data;
using NativeBackendApp.Models;

namespace NativeBackend.Services;

public class GetUserFromToken
{
    private readonly DataContext _context;

    
    public GetUserFromToken(DataContext context )
    {
        _context = context;
    }
    
    public User GetUser(HttpContext httpContext)
    {
        var token = httpContext.User;
        
        var user = _context.Users.Single(user => user.Id.ToString() == token.FindFirst("Id")!.Value);

        return user;
    }
}