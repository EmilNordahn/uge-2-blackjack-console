class Game
{
    Deck Cards = new();
    Player Dealer = new();
    Player Player = new(100.0);
    double CurrentBet = 0.0;
    double InsuranceBet = 0.0;

    public void StartNewGame()
    {
        Cards.Cards = Deck.ShuffledDeck();

        Console.WriteLine($"New game starting! You have {Player.Money} money.");

        // Validate and set the bet amount
        CurrentBet = PlayerBet();


        // Deal two cards to player and dealer
        Player.AddCard(Cards.DrawCard());
        Dealer.AddCard(Cards.DrawCard());
        Player.AddCard(Cards.DrawCard());
        Dealer.AddCard(Cards.DrawCard());


        // Check for blackjack scenario as well as insurance scenario
        CalculateBlackjackOutcome();


        // Check for split scenario
        if (Player.Hand[0].Face == Player.Hand[1].Face)
        {
            // Offer player the option to split
            Split();
            Console.WriteLine("Split opportunity!");
        }


        // Check for double down scenario
        if (Player.Money >= CurrentBet)
        {
            DoubleDown();
        }

        // Begin player turn

        PlayerPlay();
    }

    public void PlayerPlay()
    {
        Thread.Sleep(500);
        Console.WriteLine($"Your hand: {string.Join(", ", Player.Hand.Select(card => card.ToString()))} (Value: {Player.GetHandValue()})");
        Thread.Sleep(500);
        Console.WriteLine($"Dealer's visible card: {Dealer.Hand[0]}");
        Thread.Sleep(500);
        Console.WriteLine("Do you want to (h)it or (s)tand?");
        string? input = Console.ReadLine().ToLower();

        if (input == "h" || input == "hit")
        {
            PlayerHit();
            if (Player.GetHandValue() <= 21)
            {
                PlayerPlay();
            }
        }
        else if (input == "s" || input == "stand")
        {
            Stand();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'h' to hit or 's' to stand.");
            PlayerPlay();
        }
    }

    public void PlayerHit()
    {
        Thread.Sleep(500);
        Player.AddCard(Cards.DrawCard());
        Console.WriteLine($"You hit and received: {Player.Hand.Last()}. Your hand is now: {string.Join(", ", Player.Hand.Select(card => card.ToString()))} (Value: {Player.GetHandValue()})");
        if (Player.GetHandValue() > 21)
        {
            PlayerBust();
        }
    }

    public void Stand()
    {
        Thread.Sleep(500);
        Console.WriteLine($"You chose to stand with your current hand of {string.Join(",", Player.Hand.Select(card => card.ToString()))}. With a value of {Player.GetHandValue()}.");
        DealerPlay();
    }

    public void DealerPlay()
    {
        Thread.Sleep(500);
        Console.WriteLine("Dealer's turn.");
        Thread.Sleep(250);
        Console.WriteLine($"Dealer's hand: {string.Join(", ", Dealer.Hand.Select(card => card.ToString()))} (Value: {Dealer.GetHandValue()})");
        while (Dealer.GetHandValue() < 17)
        {
            Dealer.AddCard(Cards.DrawCard());
            Console.WriteLine($"Dealer hits: {string.Join(", ", Dealer.Hand.Select(card => card.ToString()))} (Value: {Dealer.GetHandValue()})");
            Thread.Sleep(500);
            if (Dealer.GetHandValue() > 21)
            {
                DealerBust();
                return;
            }
        }
        // Dealer stands, compare hands
        Console.WriteLine($"Dealer stands with a hand value of {Dealer.GetHandValue()}.");
        CalculateOutcome();
    }

    public bool OfferInsurance()
    {
        Thread.Sleep(500);
        Console.WriteLine("Dealer's visible card is an Ace. Do you want to buy insurance? (y/n)");
        string? input = Console.ReadLine().ToLower();

        if (input == "y" || input == "yes")
        {
            if (Player.Money >= CurrentBet / 2)
            {
                InsuranceBet = CurrentBet / 2;
                Player.Money -= InsuranceBet;
                Console.WriteLine($"Insurance bet of {InsuranceBet} placed. You have {Player.Money} money left.");
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
            return OfferInsurance();
        }
    }

    public void DoubleDown()
    {
        Thread.Sleep(500);
        Console.WriteLine("Do you want to double down? (y/n)");
        string? input = Console.ReadLine().ToLower();
        if (input == "y" || input == "yes")
        {
            Console.WriteLine(Player.Money + " " + CurrentBet);
            // Continue with double down logic
            if (Player.Money >= CurrentBet)
            {
                // Double the current bet
                Player.Money -= CurrentBet;
                CurrentBet *= 2;
                Console.WriteLine($"Your bet has been doubled to {CurrentBet}. You have {Player.Money} money left.");
                // Player gets exactly one more card
                Player.AddCard(Cards.DrawCard());
                Console.WriteLine($"Your hand: {string.Join(", ", Player.Hand.Select(card => card.ToString()))} (Value: {Player.GetHandValue()})");
                Thread.Sleep(500);
                if (Player.GetHandValue() > 21)
                {
                    PlayerBust();
                }
                else
                {
                    Stand();
                }
            }
            else
            {
                Console.WriteLine("You do not have enough money to double down.");
                PlayerPlay();
            }
        }
        else if (input == "n" || input == "no")
        {
            // Continue with normal play
            PlayerPlay();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
            DoubleDown();
        }
    }

    public void Split()
    {
        // Implement split logic here
    }

    public void Push()
    {
        Thread.Sleep(500);
        // Pushing returns the player's bet
        Player.Money += CurrentBet;
        Console.WriteLine("It's a push! Your bet has been returned.");
        GameEnd();
    }

    public void PlayerBust()
    {
        Thread.Sleep(500);
        Console.WriteLine($"You busted with a hand value of {Player.GetHandValue()}!");
        PlayerLose();
    }

    public void DealerBust()
    {
        Thread.Sleep(500);
        Console.WriteLine($"Dealer busted with a hand value of {Dealer.GetHandValue()}!");
        PlayerWin();
    }

    public void PlayerLose()
    {
        Thread.Sleep(500);
        // Player loses their bet, which has already been deducted
        Console.WriteLine("You lost this round.");
        GameEnd();
    }

    public void PlayerWin()
    {
        Thread.Sleep(500);
        // Implement player win logic here
        Console.WriteLine($"You win this round with a hand value of {Player.GetHandValue()}!");
        Player.Money += CurrentBet * 2;
        GameEnd();
    }

    public void PlayerBlackjack()
    {
        Thread.Sleep(500);
        // Blackjack pays 3:2
        Player.Money += (int)(CurrentBet * 1.5);
        Console.WriteLine("Blackjack! You win 1.5 times your bet.");
        GameEnd();
    }


    public void DealerWin()
    {
        Thread.Sleep(500);
        // Implement dealer win logic here
        Console.WriteLine($"Dealer wins this round with a hand value of {Dealer.GetHandValue()}.");
        GameEnd();
    }
    public void DealerBlackjack()
    {
        Thread.Sleep(500);
        // Implement dealer blackjack logic here
        Console.WriteLine("Dealer has blackjack!");
        GameEnd();
    }

    public void CalculateBlackjackOutcome()
    {
        Thread.Sleep(500);
        int playerValue = Player.GetHandValue();
        int dealerValue = Dealer.GetHandValue();
        bool insuranceBet = false;

        Console.WriteLine($"Dealer's visible card: {Dealer.Hand[0]}");
        Thread.Sleep(1000);
        Console.WriteLine($"Your hand: {string.Join(", ", Player.Hand.Select(card => card.ToString()))} (Value: {Player.GetHandValue()})");

        if (Dealer.Hand[0].Face == 1)
        {
            // Dealer has an Ace visible, offer insurance
            insuranceBet = OfferInsurance();
        }

        if (playerValue == 21 && dealerValue == 21)
        {
            Console.WriteLine("Both you and the dealer have blackjack! It's a push.");

            // Insurance pays out even if it's a push
            if (insuranceBet)
            {
                Console.WriteLine("Your insurance bet pays 2 to 1.");
                Player.Money += InsuranceBet * 2;
            }
            Push();
        }
        else if (playerValue == 21)
        {
            PlayerBlackjack();
        }
        else if (dealerValue == 21)
        {
            if (insuranceBet)
            {
                Console.WriteLine("Your insurance bet pays 2 to 1.");
                Player.Money += InsuranceBet * 2;
            }
            DealerBlackjack();
        }
        else if (insuranceBet)
        {
            Console.WriteLine("Dealer does not have blackjack. You lose your insurance bet.");
        }
    }

    public void GameEnd()
    {
        Thread.Sleep(500);
        Console.WriteLine("Round over.");
        Thread.Sleep(500);
        // Check if player has run out of money
        if (Player.Money <= 0)
        {
            Console.WriteLine("You have run out of money! Game over.");
            Environment.Exit(0);
        }

        // Reset game state for a new round
        CurrentBet = 0;
        InsuranceBet = 0;
        Player.Hand.Clear();
        Dealer.Hand.Clear();
        Player.SplitHand?.Clear();


        Console.WriteLine($"You now have {Player.Money} money.");
        Console.WriteLine("Do you want to play again? (y/n)");
        string? input = Console.ReadLine().ToLower();
        if (input == "y" || input == "yes")
        {
            StartNewGame();
        }
        else if (input == "n" || input == "no")
        {
            Console.WriteLine("Thanks for playing!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
            GameEnd();
        }
    }
    public double PlayerBet()
    {
        Thread.Sleep(500);
        // Prompt the player for a bet and deduct it from their money
        Console.WriteLine($"You have {Player.Money} money.\nPlease enter your bet amount:");
        string? betInput = Console.ReadLine();
        if (double.TryParse(betInput, out double betAmount))
        {
            if (betAmount > 0 && betAmount <= Player.Money)
            {
                Player.Money -= betAmount;
                return betAmount;
            }
            else
            {
                Console.WriteLine("Invalid bet amount. Please enter a positive number within your available money.");
                return PlayerBet();
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for your bet.");
            return PlayerBet();
        }
    }

    public void CalculateOutcome()
    {
        int playerValue = Player.GetHandValue();
        int dealerValue = Dealer.GetHandValue();

        if (playerValue > dealerValue)
        {
            PlayerWin();
        }
        else if (playerValue < dealerValue)
        {
            DealerWin();
        }
        else
        {
            Push();
        }
    }
}