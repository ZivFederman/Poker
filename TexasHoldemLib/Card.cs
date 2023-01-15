namespace TexasHoldemLib
{
    public class Card
    {
        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public Card(CardRank rank, CardSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }

    public enum CardRank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum CardSuit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }
}