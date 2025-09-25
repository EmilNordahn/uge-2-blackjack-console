class Hand
{
    public List<Card> Cards { get; set; } = [];
    public double Bet { get; set; } = 0;

    public void Add(Card card)
    {
        Cards.Add(card);
    }

    // Display the hand as a string with card names and total value
    public string Show()
    {
        return string.Join(", ", Cards.Select(card => card.ToString())) + $" (Value: {GetValue()})";
    }

    // Calculate the total value of the hand, accounting for aces
    public int GetValue()
    {
        int total = 0;

        // Sort cards by face value in descending order to handle aces last
        Cards.Sort((x, y) => -x.Face.CompareTo(y.Face));

        // Calculate the total value of the hand
        foreach (Card card in Cards)
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
}