class Card
{
    //Suit represents the suit of the card, 1-4 (1 is hearts, 2 is diamonds, 3 is clubs, 4 is spades)
    public int Suit { get; set; }
    //Face represents the number on the card, 1-13 (1 is ace, 11 is jack, 12 is queen, 13 is king)
    public int Face { get; set; }


    // Calculate the total value of a hand of cards
    public static int GetValue(List<Card> cards)
    {
        int total = 0;

        // Sort cards by face value in descending order to handle aces last
        cards.Sort((x, y) => -x.Face.CompareTo(y.Face));

        // Calculate the total value of the hand
        foreach (Card card in cards)
        {
            if (card.Face > 1 && card.Face < 11)
                total += card.Face;
            // Face cards (jack, queen, king) are worth 10
            else if (card.Face >= 11)
            {
                total += 10;
            }
            // Ace handling
            else if (card.Face == 1)
            {
                if (total + 11 > 21)
                    total += 1;
                else
                    total += 11;
            }
        }
        return total;
    }

    public override string ToString()
    {
        string[] faces = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        return $"{faces[Face - 1]} of {suits[Suit - 1]}";
    }
}