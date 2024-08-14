

using Microsoft.EntityFrameworkCore;

using var db = new GameContext();

var cards = db.Cards.ToList();

if (cards.Count == 0)
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
    db.Decks.Add(standardDeck);
    db.SaveChanges();
}

// Print all the cards in the database
Console.WriteLine(">> Todas las cartas:");
foreach (var card in db.Cards.ToList())
{
    Console.WriteLine($"> {card.Name} de {card.Suit}");
}

// Read Decks from the database using direct SQL
var decks = from d in db.Decks
            select new
            {
                d.Name,
                Cards = (from c in db.Cards
                        where c.DeckId == d.DeckId
                        select c).ToList()
            };

Console.WriteLine("***********************");
Console.WriteLine("Barajas y sus cartas:");
foreach (var deck in decks)
{
    Console.WriteLine($">> Baraja: {deck.Name}");
    foreach (var card in deck.Cards)
    {
        Console.WriteLine($">>>> {card.Name} de {card.Suit}");
    }
}

cards = db.Cards.FromSqlRaw("SELECT * FROM Cards WHERE Suit = 'Oros'").ToList();
Console.WriteLine("***********************");
Console.WriteLine("Cartas de Oros:");
foreach (var card in cards)
{
    Console.WriteLine($"> {card.Name} de {card.Suit}");
}