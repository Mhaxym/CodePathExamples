using Microsoft.EntityFrameworkCore;

public class GameContext : DbContext
{
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Card> Cards { get; set; }

    public string DbPath { get; }

    public GameContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "game.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    // If we use DbPath, the db will be created in /home/username/.local/share/game.db
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=game.db");
}

public class Deck
{
    public int DeckId { get; set; }
    public string Name { get; set; }
    public List<Card> Cards { get; set; }
}

public class Card
{
    public int CardId { get; set; }
    public string Name { get; set; }
    public string Suit { get; set; }
    public string Value { get; set; }

    // Foreign key for the Deck that this card belongs to
    public int DeckId { get; set; }
    public Deck Deck { get; set; }
}