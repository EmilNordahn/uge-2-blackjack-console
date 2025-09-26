class Game
{
    readonly Deck Cards = new();
    readonly Player Dealer = new();
    Player Player = new();

    public void InitializeGame()
    {
        Console.WriteLine("New game started! Please enter starting money");
        while (true)
        {
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double money))
            {
                if (money <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a postive numeric value for your starting money");
                    continue;
                }
                Player = new Player(money);
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric value for your starting money.");
            }
        }

        StartNewGame();
    }

    public void StartNewGame()
    {
        Cards.Cards = Deck.ShuffledDeck();
        Player.SplitHand = null;

        Console.WriteLine($"New round starting! You have {Player.Money} money.");

        // Validate and set the bet amount
        Player.Bet();


        // Deal two cards to player and dealer
        Player.MainHand.Add(Cards.DrawCard());
        Dealer.MainHand.Add(Cards.DrawCard());
        Player.MainHand.Add(Cards.DrawCard());
        Dealer.MainHand.Add(Cards.DrawCard());

        // For testing purposes, set player hand to a pair of twos
        // Player.MainHand.Cards[0] = new Card { Face = 2, Suit = 1 };
        // Player.MainHand.Cards[1] = new Card { Face = 2, Suit = 2 };

        // For testing purposes, set player hand to an Ace and a ten
        // Player.MainHand.Cards[0] = new Card { Face = 1, Suit = 1 };
        // Player.MainHand.Cards[1] = new Card { Face = 10, Suit = 2 };

        // For testing purposes, set dealer hand to an Ace and a ten
        // Dealer.MainHand.Cards[0] = new Card { Face = 1, Suit = 1 };
        // Dealer.MainHand.Cards[1] = new Card { Face = 10, Suit = 2 };

        // For testing purposes, set dealer hand to an ace and a nine
        // Dealer.MainHand.Cards[0] = new Card { Face = 1, Suit = 1 };
        // Dealer.MainHand.Cards[1] = new Card { Face = 9, Suit = 2 };


        // Check for blackjack scenario as well as insurance scenario
        CalculateBlackjackOutcome();

        // Immediately end the round if either player or dealer has blackjack
        if (Player.MainHand.GetValue() == 21 || Dealer.MainHand.GetValue() == 21)
        {
            GameEnd();
            return;
        }

        // Check for split scenario
        if (Player.MainHand.Cards[0].Face == Player.MainHand.Cards[1].Face)
        {
            // Offer player the option to split
            Split();
        }

        Player.MainHand = PlayHand(Player.MainHand);

        if (Player.SplitHand != null)
        {
            Console.WriteLine("Playing split hand.");
            Player.SplitHand = PlayHand(Player.SplitHand);
        }

        DealerPlay();

        CalculateOutcome();

        GameEnd();
    }

    public Hand PlayHand(Hand hand)
    {
        bool playing = true;
        Console.WriteLine($"\nYour hand is: " + hand.Show());

        while (playing)
        {
            // If the hand has only two cards, offer the option to double down as it's only allowed on the initial turn
            if (hand.Cards.Count == 2)
            {
                Console.WriteLine("Do you want to (h)it, (s)tand, (d)ouble down?");

                string? input = Console.ReadLine();
                input = (input != null) ? input.ToLower() : "";
                if (input == "h" || input == "hit")
                {
                    hand.Add(Cards.DrawCard());
                    Console.WriteLine($"You hit and received: {hand.Cards.Last()}. Your hand is now: " + hand.Show());
                    if (hand.GetValue() > 21)
                    {
                        Console.WriteLine("Player BUST!");
                        return hand;
                    }
                }
                else if (input == "s" || input == "stand")
                {
                    Console.WriteLine($"You stood on {hand.GetValue()}!");
                    return hand;
                }
                else if (input == "d" || input == "double down")
                {
                    // Double the bet, take one card, and end the turn
                    if (Player.Money < hand.Bet)
                    {
                        Console.WriteLine("Not enough money to double down");
                        continue;
                    }
                    else
                    {
                        Player.Money -= hand.Bet;
                        hand.Bet *= 2;
                        hand.Add(Cards.DrawCard());
                        Console.WriteLine($"You doubled down and received: {hand.Cards.Last()}. Your hand is now: " + hand.Show());
                        if (hand.GetValue() > 21)
                        {
                            Console.WriteLine("Player BUST!");
                        }
                        return hand;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'h' for hit, 's' for stand, or 'd' for double down.");
                }
            }
            // If the hand has more than two cards, only allow hit or stand as double down is not allowed after the initial turn
            else
            {
                if (hand.GetValue() == 21)
                {
                    // Automatically end turn if player has 21
                    Console.WriteLine("Standing on 21!");
                    playing = false;
                    continue;
                }
                Console.WriteLine("Do you want to (h)it or (s)tand?");
                string? input = Console.ReadLine();
                input = (input != null) ? input.ToLower() : "";
                if (input == "h" || input == "hit")
                {
                    hand.Add(Cards.DrawCard());
                    Console.WriteLine($"You hit and received: {hand.Cards.Last()}. Your hand is now: " + hand.Show());
                    if (hand.GetValue() > 21)
                    {
                        Console.WriteLine("Player BUST!");
                        return hand;
                    }
                }
                else if (input == "s" || input == "stand")
                {
                    Console.WriteLine($"You stood on {hand.GetValue()}!");
                    return hand;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'h' for hit or 's' for stand.");
                }
            }
        }
        // Ensure a return value for all code paths, however this should never be reached
        return hand;
    }

    public void DealerPlay()
    {
        // Dealer must hit until their hand value is 17 or higher
        Thread.Sleep(500);
        Console.WriteLine($"Dealer's hand is: " + Dealer.MainHand.Show());
        while (Dealer.MainHand.GetValue() < 17)
        {
            Dealer.MainHand.Add(Cards.DrawCard());
            Console.WriteLine($"Dealer hits and receives: {Dealer.MainHand.Cards.Last()}. Dealer's hand is now: " + Dealer.MainHand.Show());
            Thread.Sleep(500);
        }
        if (Dealer.MainHand.GetValue() > 21)
        {
            Console.WriteLine("Dealer BUST!");
            Thread.Sleep(500);
        }
        else
        {
            Console.WriteLine("Dealer stands.");
            Thread.Sleep(500);
        }
    }

    public void Split()
    {
        // Offer player the option to split if they have a pair
        Console.WriteLine("You have a pair! Do you want to split? (y/n)");
        // Get and validate user input
        string? input = Console.ReadLine();
        input = (input != null) ? input.ToLower() : "";
        if (input == "y" || input == "yes")
        {
            // Ensure player has enough money to split
            if (Player.Money >= Player.MainHand.Bet)
            {
                Player.Money -= Player.MainHand.Bet;
                // Declare and initialize the split hand
                Player.SplitHand = new Hand { Bet = Player.MainHand.Bet };
                // Move one of the cards from the main hand to the split hand
                Player.SplitHand.Add(Player.MainHand.Cards[1]);
                Player.MainHand.Cards.RemoveAt(1);
                // Deal one additional card to each hand
                Player.MainHand.Add(Cards.DrawCard());
                Player.SplitHand.Add(Cards.DrawCard());
                // Show both hands
                Console.WriteLine($"Hands after split:\nMain Hand: {Player.MainHand.Show()}\nSplit Hand: {Player.SplitHand.Show()}");
            }
            else
            {
                Console.WriteLine("Not enough money to split.");
            }
        }
        else if (input == "n" || input == "no")
        {
            Console.WriteLine("No split taken.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
            Split();
        }
    }

    public void CalculateBlackjackOutcome()
    {
        Thread.Sleep(500);
        bool insuranceBet = false;

        Console.WriteLine($"Dealer's visible card: {Dealer.MainHand.Cards[0]}");
        Thread.Sleep(1000);
        Console.WriteLine($"Your hand: " + Player.MainHand.Show());

        if (Dealer.MainHand.Cards[0].Face == 1)
        {
            // Dealer has an Ace visible, offer insurance
            insuranceBet = Player.OfferInsurance();
        }

        if (Player.MainHand.GetValue() == 21 && Dealer.MainHand.GetValue() == 21)
        {
            Console.WriteLine("Both you and the dealer have blackjack! It's a push.");

            // Insurance pays out even if it's a push
            if (insuranceBet)
            {
                Console.WriteLine("Your insurance bet pays 2 to 1.");
                Player.Money += Player.InsuranceBet * 3;
            }
            Player.Money += Player.MainHand.Bet;
        }
        else if (Player.MainHand.GetValue() == 21)
        {
            // Player has blackjack, but dealer does not.
            Console.WriteLine("Blackjack! You win 1.5x your bet plus your original bet.");
            Player.Money += Player.MainHand.Bet * 2.5;
        }
        else if (Dealer.MainHand.GetValue() == 21)
        {
            Console.WriteLine($"Dealer has a blackjack! \nDealer's hand: {Dealer.MainHand.Show()}");
            if (insuranceBet)
            {
                Console.WriteLine("Your insurance bet pays 2 to 1.");
                Player.Money += Player.InsuranceBet * 3;
            }
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
        Player.InsuranceBet = 0.0;
        Player.MainHand.Cards.Clear();
        Dealer.MainHand.Cards.Clear();
        Player.SplitHand?.Cards.Clear();


        Console.WriteLine($"You now have {Player.Money} money.");
        Console.WriteLine("Do you want to play again? (y/n)");
        while (true)
        {
            string? input = Console.ReadLine();
            input = (input != null) ? input.ToLower() : "";
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
            }
        }
    }

    public void CalculateOutcome()
    {
        // Calculate outcome for main hand
        CalculateHandOutcome(Player.MainHand);

        // Calculate outcome for split hand if it exists
        if (Player.SplitHand != null)
        {
            CalculateHandOutcome(Player.SplitHand);
        }
    }

    public void CalculateHandOutcome(Hand hand)
    {
        int playerValue = hand.GetValue();
        int dealerValue = Dealer.MainHand.GetValue();

        if (playerValue > 21)
        {
            Console.WriteLine($"You busted with a hand value of {playerValue}. You lose your bet of {hand.Bet}.");
        }
        else if (dealerValue > 21)
        {
            Console.WriteLine($"Dealer busted with a hand value of {dealerValue}. You win!");
            Player.Money += hand.Bet * 2;
        }
        else if (playerValue > dealerValue)
        {
            Console.WriteLine($"You win with a hand value of {playerValue} against dealer's {dealerValue}!");
            Player.Money += hand.Bet * 2;
        }
        else if (playerValue < dealerValue)
        {
            Console.WriteLine($"You lose with a hand value of {playerValue} against dealer's {dealerValue}. You lose your bet of {hand.Bet}.");
        }
        else
        {
            Console.WriteLine($"It's a push with both you and the dealer having a hand value of {playerValue}. Your bet of {hand.Bet} is returned.");
            Player.Money += hand.Bet;
        }
    }
}