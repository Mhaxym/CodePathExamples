

namespace CodePathWebAPI.Models;

public partial class Page
{
    public int ID { get; set; }
    public int Order { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    private string _path = string.Empty;
    public string Path
    {
        get => _path;
        set => _path = CreatePath(value);
    }
    public string Content { get; set; } = string.Empty;

    // Self-referencing relationship
    public int? ParentPageID { get; set; }
    public Page? ParentPage { get; set; }
    public List<Page>? Children { get; set; }
    public static string CreatePath(string value) => value.StartsWith("/c/") ? value : $"/c/{value}";
}
