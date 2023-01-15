using TexasHoldemLib;

namespace HoldemTests
{
    public class DealerTests
    {
        private const int DECK_CARD_AMOUNT = 52;

        [Fact]
        public void CardsPerDeckTest()
        {
            CardDealer dealer = new CardDealer();
            List<Card> cards = dealer.CreateDeck(shouldShuffle: false);
            Assert.Equal(DECK_CARD_AMOUNT, cards.Count);
        }

        [Fact]
        public void ShuffleRngTest()
        {
            CardDealer firstDealer = new CardDealer();
            CardDealer secondDealer = new CardDealer();

            List<Card> firstDeck = firstDealer.CreateDeck(shouldShuffle: true);
            Thread.Sleep(100);
            List<Card> secondDeck = secondDealer.CreateDeck(shouldShuffle: true);

            bool isDifferentOrder = false;
            for (int i = 0; i < firstDeck.Count; i++)
            {
                if (firstDeck[i].Rank != secondDeck[i].Rank || firstDeck[i].Suit != secondDeck[i].Suit)
                {
                    isDifferentOrder = true;
                    break;
                }
            }

            Assert.True(isDifferentOrder);
        }
    }
}