class Game
{
    Deck Cards = new();
    Player Dealer = new();
    Player Player = new(100);
    // change this to be dynamic based on player input
    int CurrentBet = 10;

    public void StartNewGame()
    {
        Cards.Cards = Deck.ShuffledDeck(Cards.Cards);

        Player.Money -= CurrentBet;

        // Deal two cards to player and dealer
        PlayerHit();
        Dealer.AddCard(Cards.DrawCard());
        PlayerHit();
        Dealer.AddCard(Cards.DrawCard());

        // Check for insurance scenario
        if (Dealer.Hand[0].Face == 1)
        {
            OfferInsurance();
        }

        // Check for blackjack scenario
        CalculateBlackjackOutcome();

        // Check for split scenario
        if (Player.Hand[0].Face == Player.Hand[1].Face)
        {
            // Offer player the option to split
            Split();
        }

        // Check for double down scenario
        if (Player.Money >= CurrentBet)
        {
            // Offer player the option to double down
            DoubleDown();
        }

        // Begin player turn
        PlayerPlay();
    }

    public void PlayerPlay()
    {
        // Implement player turn logic here (e.g., hit, stand)
    }

    public void PlayerHit()
    {
        Player.AddCard(Cards.DrawCard());
        if (Player.GetHandValue() > 21)
        {
            PlayerBust();
        }
    }

    public void Stand()
    {
        DealerPlay();
    }

    public void DealerPlay()
    {
        while (Dealer.GetHandValue() < 17 && Dealer.GetHandValue() <= Player.GetHandValue())
        {
            Dealer.AddCard(Cards.DrawCard());
        }
    }

    public static void OfferInsurance()
    {
        // Implement insurance logic here
    }

    public void DoubleDown()
    {
        // Implement double down logic here
    }

    public void Split()
    {
        // Implement split logic here
    }

    public void Push()
    {
        // Implement push logic here
        Player.Money += CurrentBet;
    }

    public void PlayerBust()
    {
        // Implement player bust logic here
    }

    public void DealerBust()
    {
        PlayerWin();
    }

    public void PlayerLose()
    {
        // Implement player lose logic here
    }

    public void PlayerWin()
    {
        // Implement player win logic here
        Player.Money += CurrentBet * 2;
    }

    public void PlayerBlackjack()
    {
        // Implement player blackjack logic here
        Player.Money += (int)(CurrentBet * 2.5);
    }


    public void DealerWin()
    {
        // Implement dealer win logic here
    }
    public void DealerBlackjack()
    {
        // Implement dealer blackjack logic here
    }

    public void CalculateBlackjackOutcome()
    {
        // Implement outcome calculation logic here
        if (Player.GetHandValue() == 21 && Dealer.GetHandValue() == 21)
        {
            Push();
        }
        else if (Player.GetHandValue() == 21 && Dealer.GetHandValue() != 21)
        {
            PlayerBlackjack();
        }
        else if (Dealer.GetHandValue() == 21 && Player.GetHandValue() != 21)
        {
            DealerBlackjack();
        }
    }
}