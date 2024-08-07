using System.Text.Json.Serialization;
using CodePathAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddNpgsql<NetCoreDbContext>(connectionString);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CodePathAPI";
    config.Title = "CodePathAPI v1";
    config.Version = "v1";
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "CodePathAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// Direct routes

app.MapGet(
    "/Page",
    async (NetCoreDbContext db) => await db.Pages.ToListAsync()
);
app.MapGet(
    "/Page/{id}",
    async (NetCoreDbContext db, int id) => await db.Pages.FindAsync(id)
);


// Function routes
var userPagesRouter = app.MapGroup("/UserPage");

userPagesRouter.MapGet("/", GetAllUserPages);
userPagesRouter.MapGet("/{id}", GetUserPage);

static async Task<IResult> GetAllUserPages(NetCoreDbContext db)
{
    return TypedResults.Ok(await db.UserPages.ToListAsync());
}

static async Task<IResult> GetUserPage(NetCoreDbContext db, int id)
{
    return await db.UserPages.FindAsync(id)
        is UserPage userPage
            ? TypedResults.Ok(userPage)
            : TypedResults.NotFound();
}

app.Run();
