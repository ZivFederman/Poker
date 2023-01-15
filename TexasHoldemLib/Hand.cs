namespace TexasHoldemLib
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        // The rank relevant cards in a pair would be only the cards making the pair
        public List<Card> RankRelevantCards { get; set; }
        public HandRank Rank { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
            Rank = HandRank.Unknown;
        }

        public Hand(List<Card> cards)
        {
            Cards = cards;
            Rank = HandRank.Unknown;
        }

        public Hand(List<Card> cards, HandRank rank)
        {
            Cards = cards;
            Rank = rank;
        }

        public Hand(List<Card> cards, HandRank rank, List<Card> rankRelevantCards)
        {
            Cards = cards;
            Rank = rank;
            RankRelevantCards = rankRelevantCards;
        }
    }

    public enum HandRank
    {
        Unknown,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }
}
