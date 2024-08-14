using Dapper;

var connectionString = $"Data Source=game.db";

// Run migrations
MigrationRunner.RunMigrations(connectionString);

// Use Dapper to interact with the database
var context = new GameContext(connectionString);

var decks = context.GetDecks();
var cards = context.GetCards();

if (!decks.Any() && !cards.Any())
{
     // Create a standard deck with all the cards
    var standardDeck = new Deck { Name = "Española", Cards = new List<Card>() };
    foreach (var suit in new string[] { "Oros", "Copas", "Espadas", "Bastos" })
    {
        standardDeck.Cards.Add(new Card { Name = "As", Suit = suit, Value = "1" });
        for (int j = 2; j <= 9; j++)
        {
            standardDeck.Cards.Add(new Card { Name = $"{j}", Suit = suit, Value = j.ToString() });
        }
        standardDeck.Cards.Add(new Card { Name = "Sota", Suit = suit, Value = "10" });
        standardDeck.Cards.Add(new Card { Name = "Caballo", Suit = suit, Value = "11" });
        standardDeck.Cards.Add(new Card { Name = "Rey", Suit = suit, Value = "12" });
    }
    if (standardDeck.Cards.Count > 0)
    {
        using var connection = context.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var deckId = connection.ExecuteScalar<long>(
                "INSERT INTO Decks (Name) VALUES (@Name); SELECT last_insert_rowid();",
                new { standardDeck.Name },
                transaction
            );

            foreach (var card in standardDeck.Cards)
            {
                card.DeckId = deckId;
            }

            connection.Execute(
                "INSERT INTO Cards (Name, Suit, Value, DeckId) VALUES (@Name, @Suit, @Value, @DeckId);",
                standardDeck.Cards,
                transaction
            );

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}

decks = context.GetDecks();

foreach (var deck in decks)
{
    Console.WriteLine($"Deck: {deck.Name}");
    foreach (var card in deck.Cards)
    {
        Console.WriteLine($"  {card.Name} of {card.Suit}");
    }
}