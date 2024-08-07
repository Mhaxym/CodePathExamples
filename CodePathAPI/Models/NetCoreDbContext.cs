using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePathAPI.Models;

public class NetCoreDbContext(DbContextOptions<NetCoreDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Page> Pages { get; set; }

    public DbSet<UserPage> UserPages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Page>()
            .ToTable("Page")
            .HasOne(p => p.ParentPage)
            .WithMany(p => p.Children)
            .HasForeignKey(p => p.ParentPageID)
            .OnDelete(DeleteBehavior.Restrict); // Prevents cascading deletes
        modelBuilder.Entity<UserPage>().ToTable("UserPage");
    }
}
