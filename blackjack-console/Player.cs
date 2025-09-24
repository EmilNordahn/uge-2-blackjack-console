class Player
{
    public int? Money { get; set; }
    public List<Card> Hand { get; set; } = [];
    public List<Card>? SplitHand { get; set; } = [];

    public Player(int startingMoney)
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