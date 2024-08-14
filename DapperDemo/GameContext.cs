using System.Data;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib.Extensions;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

public class GameContext
{
    private readonly string _connectionString;

    public GameContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SQLiteConnection(_connectionString);

    public Deck GetDeck(int deckId)
    {
        using var connection = CreateConnection();
        return connection.Get<Deck>(deckId);
    }

    public long AddDeck(Deck deck)
    {
        using var connection = CreateConnection();
        return connection.Insert(deck);
    }

    public long AddCard(Card card)
    {
        using var connection = CreateConnection();
        return connection.Insert(card);
    }

    public IEnumerable<Deck> GetDecks()
    {
        using var connection = CreateConnection();
        var decks = connection.Query<Deck>("SELECT * FROM Decks");      
        foreach (var deck in decks)
        {
            deck.Cards = connection.Query<Card>("SELECT * FROM Cards WHERE DeckId = @DeckId", new { deck.DeckId }).ToList();
        }
        return decks;  
    }

    public IEnumerable<Card> GetCards()
    {
        using var connection = CreateConnection();
        return connection.Query<Card>("SELECT * FROM Cards");
    }
}

public class Deck
{
    public long DeckId { get; set; }
    public string Name { get; set; }

    [Write(false)]
    public List<Card> Cards { get; set; }
}

public class Card
{
    public long CardId { get; set; }
    public string Name { get; set; }
    public string Suit { get; set; }
    public string Value { get; set; }

    // Foreign key for the Deck that this card belongs to
    public long DeckId { get; set; }
    [Write(false)]
    public Deck Deck { get; set; }
}


// FluentMigrator Migrations
[Migration(1)]
public class InitialCreate : Migration
{
    public override void Up()
    {
        Create.Table("Decks")
            .WithColumn("DeckId").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString();

        Create.Table("Cards")
            .WithColumn("CardId").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString()
            .WithColumn("Suit").AsString()
            .WithColumn("Value").AsString()
            .WithColumn("DeckId").AsInt64().ForeignKey("Decks", "DeckId");
    }

    public override void Down()
    {
        Delete.Table("Cards");
        Delete.Table("Decks");
    }
}


public static class MigrationRunner
{
    public static void RunMigrations(string connectionString)
    {
        var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialCreate).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}