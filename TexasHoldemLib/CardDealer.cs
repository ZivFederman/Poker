using System;
using System.Collections.Generic;

namespace TexasHoldemLib
{
    public class CardDealer
    {
        public List<Card> CreateDeck(bool shouldShuffle = true)
        {
            List<Card> deck = new List<Card>();
            foreach (CardRank rank in (CardRank[])Enum.GetValues(typeof(CardRank)))
            {
                foreach (CardSuit suit in (CardSuit[])Enum.GetValues(typeof(CardSuit)))
                {
                    deck.Add(new Card(rank, suit));
                }
            }

            if (shouldShuffle)
            {
                deck = ShuffleDeck(deck);
            }

            return deck;
        }

        public List<Card> ShuffleDeck(List<Card> deck)
        {
            int n = deck.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                // Swaps two cards: deck[k] and deck[n]
                Card card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }

            return deck;
        }
    }
}