
using Microsoft.AspNetCore.Authorization;

namespace CodePathWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPageController(ILogger<UserPageController> logger, NetCoreDbContext db) : ControllerBase
{
    private readonly ILogger<UserPageController> _logger = logger;
    private readonly NetCoreDbContext _db = db;

    [HttpGet(Name = "GetUserPages")]
    public async Task<IResult> Get()
    {
        return TypedResults.Ok(await _db.UserPages.ToListAsync());
    }

    [Authorize]
    [HttpGet("{id}", Name = "GetUserPage")]
    public async Task<IResult> Get(int id)
    {
        return await _db.UserPages.FindAsync(id)
            is UserPage userPage
                ? TypedResults.Ok(userPage)
                : TypedResults.NotFound();
    }
}
