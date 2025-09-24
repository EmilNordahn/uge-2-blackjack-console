class Deck
{
    public List<Card> Cards = [];
    public static List<Card> GetFullDeck()
    {
        // Create a standard deck of 52 cards
        List<Card> cards = new List<Card>();
        for (int suit = 1; suit <= 4; suit++)
        {
            // For each suit, create cards with faces 1-13
            for (int face = 1; face <= 13; face++)
            {
                // Create a new card and set its properties
                Card card = new Card();
                card.Suit = suit;
                card.Face = face;
                // Set the value face cards (jack, queen, king) to 10
                if (face > 10)
                    card.Value = 10;
                else
                    card.Value = face;
                cards.Add(card);
            }
        }
        return cards;
    }

    public static List<Card> ShuffledDeck(List<Card> cards)
    {
        // Shuffle the deck of cards using Fisher-Yates shuffle algorithm
        Random rand = new Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            // Swap cards[i] with the element at random index
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
        return cards;
    }

    public Card DrawCard()
    {
        if (Cards.Count == 0)
            throw new InvalidOperationException("No cards left in the deck.");
        Card drawnCard = Cards[0];
        Cards.RemoveAt(0);
        return drawnCard;
    }

    public Deck()
    {
        Cards = GetFullDeck();
    }


}