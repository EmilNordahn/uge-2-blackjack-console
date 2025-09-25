class Player
{
    public double? Money { get; set; }
    public Hand MainHand { get; set; } = new Hand();
    public Hand? SplitHand { get; set; }

    public Player(double startingMoney)
    {
        Money = startingMoney;
    }

    public Player() { }
}