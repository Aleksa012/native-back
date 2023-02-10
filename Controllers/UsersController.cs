using Microsoft.AspNetCore.Mvc;
using NativeBackendApp.Data;

namespace NativeBackendApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly DataContext _context;
    
    public UsersController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok(_context.Users);
    }
}