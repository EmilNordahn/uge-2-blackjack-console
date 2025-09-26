class Player
{
    public double? Money { get; set; }
    public Hand MainHand { get; set; } = new Hand();
    public Hand? SplitHand { get; set; }
    public double InsuranceBet { get; set; } = 0;

    public Player(double startingMoney)
    {
        Money = startingMoney;
    }

    public Player() { }

    public bool OfferInsurance()
    {
        Thread.Sleep(500);
        Console.WriteLine("Dealer's visible card is an Ace. Do you want to buy insurance? (y/n)");

        while (true)
        {
            string? input = Console.ReadLine();
            input = (input != null) ? input.ToLower() : "";

            if (input == "y" || input == "yes")
            {
                // Insurance bet is half the original bet
                if (Money >= MainHand.Bet / 2)
                {
                    InsuranceBet = MainHand.Bet / 2;
                    Money -= InsuranceBet;
                    Console.WriteLine($"Insurance bet of {InsuranceBet} placed. You have {Money} money left.");
                    Thread.Sleep(500);
                    return true;
                }
                else
                {
                    Console.WriteLine("You do not have enough money to place an insurance bet.");
                    Thread.Sleep(500);
                    return false;
                }
            }
            else if (input == "n" || input == "no")
            {
                Console.WriteLine("No insurance taken.");
                Thread.Sleep(500);
                return false;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
            }
        }
    }
}