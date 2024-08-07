
namespace CodePathWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PageController(ILogger<PageController> logger, NetCoreDbContext db) : ControllerBase
{
    private readonly ILogger<PageController> _logger = logger;
    private readonly NetCoreDbContext _db = db;

    [HttpGet(Name = "GetPages")]
    public async Task<IResult> Get()
    {
        return TypedResults.Ok(await _db.Pages.ToListAsync());
    }

    [HttpGet("{id}", Name = "GetPage")]
    public async Task<IResult> Get(int id)
    {
        return await _db.Pages.FindAsync(id)
            is Page page
                ? TypedResults.Ok(page)
                : TypedResults.NotFound();
    }
}
