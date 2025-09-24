class Player
{
    public double? Money { get; set; }
    public List<Card> Hand { get; set; } = [];
    public List<Card>? SplitHand { get; set; } = [];

    public Player(double startingMoney)
    {
        Money = startingMoney;
    }

    public Player() { }

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    public int GetHandValue()
    {
        return Card.GetValue(Hand);
    }




}