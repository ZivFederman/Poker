using TexasHoldemLib;

namespace HoldemTests
{
    public class EvaluatorTests
    {
        [Fact]
        public void RoyalFlushTest()
        {
            List<Card> cards1 = new List<Card>()
            {
                new Card(CardRank.King, CardSuit.Clubs),
                new Card(CardRank.Ten, CardSuit.Clubs),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Queen, CardSuit.Clubs),
                new Card(CardRank.Eight, CardSuit.Spades),
                new Card(CardRank.Two, CardSuit.Hearts),
                new Card(CardRank.Jack, CardSuit.Clubs),
            };

            Hand hand1 = HandEvaluator.EvalulateHandRank(cards1);
            Assert.True(hand1.Rank == HandRank.RoyalFlush);
        }

        [Fact]
        public void StraightFlushTest()
        {
            List<Card> cards1 = new List<Card>()
            {
                new Card(CardRank.Five, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Four, CardSuit.Spades),
                new Card(CardRank.Two, CardSuit.Spades),
                new Card(CardRank.Three, CardSuit.Spades),
                new Card(CardRank.Jack, CardSuit.Hearts),
                new Card(CardRank.Eight, CardSuit.Diamonds)
            };

            List<Card> cards2 = new List<Card>()
            {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.King, CardSuit.Hearts),
                new Card(CardRank.Queen, CardSuit.Clubs),
                new Card(CardRank.Jack, CardSuit.Hearts),
                new Card(CardRank.Two, CardSuit.Diamonds),
                new Card(CardRank.Five, CardSuit.Clubs),
                new Card(CardRank.Six, CardSuit.Diamonds)
            };

            Hand hand1 = HandEvaluator.EvalulateHandRank(cards1);
            Hand hand2 = HandEvaluator.EvalulateHandRank(cards2);
            Assert.True(hand1.Rank == HandRank.StraightFlush && hand2.Rank != HandRank.StraightFlush);
        }

        [Fact]
        public void FullHouseTest()
        {
            List<Card> cards1 = new List<Card>()
            {
                new Card(CardRank.Five, CardSuit.Spades),
                new Card(CardRank.Five, CardSuit.Clubs),
                new Card(CardRank.Five, CardSuit.Diamonds),
                new Card(CardRank.Eight, CardSuit.Spades),
                new Card(CardRank.Jack, CardSuit.Hearts),
                new Card(CardRank.Three, CardSuit.Diamonds),
                new Card(CardRank.Three, CardSuit.Spades)
            };

            List<Card> cards2 = new List<Card>()
            {
                new Card(CardRank.Three, CardSuit.Spades),
                new Card(CardRank.Three, CardSuit.Clubs),
                new Card(CardRank.Three, CardSuit.Diamonds),
                new Card(CardRank.Eight, CardSuit.Spades),
                new Card(CardRank.Jack, CardSuit.Hearts),
                new Card(CardRank.Five, CardSuit.Diamonds),
                new Card(CardRank.Five, CardSuit.Spades)
            };

            Hand hand1 = HandEvaluator.EvalulateHandRank(cards1);
            Hand hand2 = HandEvaluator.EvalulateHandRank(cards1);
            Assert.True(hand1.Rank == HandRank.FullHouse && hand2.Rank == HandRank.FullHouse);
        }
    }
}
