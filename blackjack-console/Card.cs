class Card
{
    //Suit represents the suit of the card, 1-4 (1 is hearts, 2 is diamonds, 3 is clubs, 4 is spades)
    public int Suit { get; set; }
    //Face represents the number on the card, 1-13 (1 is ace, 11 is jack, 12 is queen, 13 is king)
    public int Face { get; set; }


    public override string ToString()
    {
        string[] faces = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        return $"{faces[Face - 1]} of {suits[Suit - 1]}";
    }
}