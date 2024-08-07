
namespace CodePathAPI.Models;

public class UserPage
{
    public int ID { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PageID { get; set; }
    public bool Completed { get; set; }
    public DateTime? CompletedDate { get; set; }
    public ApplicationUser? User { get; set; }
    public Page? Page { get; set; }
}
