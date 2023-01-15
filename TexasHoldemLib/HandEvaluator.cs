using static TexasHoldemLib.Hand;

namespace TexasHoldemLib
{
    public static class HandEvaluator
    {
        #region constants

        public const int MAX_CARDS_FOR_HAND = 5;

        #endregion

        #region public methods

        /// <summary>
        /// Returns the strongest hand that can be made from the given cards.
        /// </summary>
        /// <param name="cards">The cards which are used to create the hand.</param>
        /// <returns>Returns the strongest hand using the given cards.</returns>
        public static Hand EvalulateHandRank(List<Card> cards)
        {
            Hand evaluatedHand = TryRoyalFlush(cards);
            if (evaluatedHand == null)
            {
                evaluatedHand = TryStraightFlush(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryFourOfAKind(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryFullHouse(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryFlush(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryStraight(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryThreeOfAKind(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryTwoPair(cards);
            }
            if (evaluatedHand == null)
            {
                evaluatedHand = TryPair(cards);
            }
            if (evaluatedHand == null)
            {
                cards = SortCardsByRank(cards);
                List<Card> highCards = cards.GetRange(0, cards.Count);
                evaluatedHand = new Hand(highCards, HandRank.HighCard, highCards);
            }
            return evaluatedHand;
        }

        /// <summary>
        /// Tries to create a hand with a royal flush from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a royal flush.</param>
        /// <returns>Returns a hand with a royal flush if possible, otherwise null.</returns>
        public static Hand TryRoyalFlush(List<Card> cards)
        {
            Hand handWithRoyalFlush = null;
            if (cards != null && cards.Count >= 5)
            {
                // Sorts the cards to ease the search
                cards = SortCardsByRank(cards);
                List<Card> flushCards = null;
                // Creates four card lists, one for each suit
                Dictionary<CardSuit, List<Card>> cardListsBySuit = new Dictionary<CardSuit, List<Card>>();
                foreach (Card card in cards)
                {
                    // Adds the suit to the dictionary if it has not been added yet
                    if (!cardListsBySuit.ContainsKey(card.Suit))
                    {
                        cardListsBySuit.Add(card.Suit, new List<Card>());
                    }
                    List<Card> suitedCardList = cardListsBySuit[card.Suit];
                    suitedCardList.Add(card);
                    // Five cards of the same suit is the first requirement (the flush)
                    if (suitedCardList.Count == 5)
                    {
                        flushCards = suitedCardList;
                        // Breaks out of the flush loop, up next is checking whether the flush is also royally straight
                        break;
                    }
                }
                if (flushCards != null)
                {
                    // Creates a list of the required ranks for a straight flush
                    List<CardRank> requiredRanks = new List<CardRank>()
                    {
                            CardRank.Ace,
                            CardRank.King,
                            CardRank.Queen,
                            CardRank.Jack,
                            CardRank.Ten
                    };
                    for (int i = 0; i < requiredRanks.Count; i++)
                    {
                        if (flushCards[i].Rank != requiredRanks[i])
                        {
                            // If the rank does not match the required rank the function ends with returning handsWithRyoalFlush as null
                            return handWithRoyalFlush;
                        }
                    }
                    handWithRoyalFlush = new Hand(flushCards, HandRank.RoyalFlush, flushCards);
                }
            }
            return handWithRoyalFlush;
        }

        /// <summary>
        /// Tries to create a hand with a straight flush from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a straight flush.</param>
        /// <returns>Returns a hand with a straight flush if possible, otherwise null.</returns>
        public static Hand TryStraightFlush(List<Card> cards)
        {
            Hand handWithStraightFlush = null;
            // Needs at least five cards to create a straight flush
            if (cards != null && cards.Count >= 5)
            {
                // Sorts by rank to ease the search
                cards = SortCardsByRank(cards);
                // Creates four card lists, one for each suit
                Dictionary<CardSuit, List<Card>> cardListsBySuit = new Dictionary<CardSuit, List<Card>>();
                foreach (Card card in cards)
                {
                    // Adds the suit to the dictionary if it has not been added yet
                    if (!cardListsBySuit.ContainsKey(card.Suit))
                    {
                        cardListsBySuit.Add(card.Suit, new List<Card>());
                    }
                    cardListsBySuit[card.Suit].Add(card);
                }
                foreach (CardSuit suit in cardListsBySuit.Keys)
                {
                    List<Card> suitCardList = cardListsBySuit[suit];
                    // If there are five cards of the same suit the requirements for flush has been completed (still need to check whether they are straight)
                    if (suitCardList.Count >= 5)
                    {
                        // Straight = five consecutive cards with correctly rising ranks
                        List<Card> consecutiveCards = new List<Card>
                        {
                            // Starts with the first card as the beginning
                            suitCardList[0]
                        };
                        for (int i = 0; i < suitCardList.Count - 1 && consecutiveCards.Count < 5; i++)
                        {
                            Card currentCard = suitCardList[i];
                            Card nextCard = suitCardList[i + 1];
                            // If the next card is a rank lower than it is consecutive in the rank order
                            if (currentCard.Rank - 1 == nextCard.Rank)
                            {
                                consecutiveCards.Add(nextCard);
                            }
                            else if (currentCard.Rank != nextCard.Rank) // If the next card's rank is not one lower or equal then the cards are not consecutive
                            {
                                // Empties the list because the order was broken and has to start anew using the current card
                                consecutiveCards.Clear();
                                consecutiveCards.Add(nextCard);
                            }
                        }
                        // Special check for ace card, because rank-wise ace it is at the top order-wise it can be the bottom card (one/ace to five)
                        if (consecutiveCards.Count == 4 && consecutiveCards[3].Rank == CardRank.Two && suitCardList[0].Rank == CardRank.Ace)
                        {
                            consecutiveCards.Add(suitCardList[0]);
                        }
                        if (consecutiveCards.Count == 5)
                        {
                            consecutiveCards = consecutiveCards.GetRange(0, 5);
                            handWithStraightFlush = new Hand(consecutiveCards, HandRank.StraightFlush, consecutiveCards);
                            break;
                        }
                    }
                }
            }
            return handWithStraightFlush;
        }

        /// <summary>
        /// Tries to create a hand with a four of a kind from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a four of a kind.</param>
        /// <returns>Returns a hand with a four of a kind followed up by the strongest leftover card if possible, otherwise null.</returns>
        public static Hand TryFourOfAKind(List<Card> cards)
        {
            Hand handWithFourOfAKind = null;
            // Needs at least four cards to create a four of a kind
            if (cards != null && cards.Count >= 4)
            {
                // Sorts the cards so it tries finding a four of a kind using the stronger cards first
                cards = SortCardsByRank(cards);
                for (int i = 0; i < cards.Count - 3; i++)
                {
                    // Cards with the same rank should be next to each other because they were sorted by rank
                    if (cards[i].Rank == cards[i + 1].Rank && cards[i].Rank == cards[i + 2].Rank && cards[i].Rank == cards[i + 3].Rank)
                    {
                        // If four matching cards were found then the requirements for a four of a kind are fulfilled
                        List<Card> fourOfAKind = new List<Card>() { cards[i], cards[i + 1], cards[i + 2], cards[i + 3] };
                        // Creates a list containing the remaining cards by removing the cards used for the four of a kind
                        List<Card> remainingCards = cards;
                        fourOfAKind.ForEach(card => remainingCards.Remove(card));
                        // Finishes the cards by adding the strongest remaining cards
                        List<Card> completedCards = CompleteCards(fourOfAKind, remainingCards);
                        handWithFourOfAKind = new Hand(completedCards, HandRank.FourOfAKind, fourOfAKind);
                        // Breaks out of the loop because there is no need to continue
                        break;
                    }
                }
            }
            return handWithFourOfAKind;
        }

        /// <summary>
        /// Tries to create a hand with a full house from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a full house.</param>
        /// <returns>Returns a hand with a full house if possible, otherwise null.</returns>
        public static Hand TryFullHouse(List<Card> cards)
        {
            Hand handWithFullHouse = null;
            if (cards != null && cards.Count > 5)
            {
                // Sorts by rank to get a stronger three of a kind and pair
                cards = SortCardsByRank(cards);
                List<Card> fullHouseCards = new List<Card>();
                bool hasPair = false;
                bool hasThreeOfAKind = false;
                for (int i = 0; i < cards.Count - 1; i++)
                {
                    if (!hasThreeOfAKind && i < cards.Count - 2 && cards[i].Rank == cards[i + 1].Rank && cards[i].Rank == cards[i + 2].Rank)
                    {
                        hasThreeOfAKind = true;
                        fullHouseCards.AddRange(cards.GetRange(i, 3));
                    }
                    else if (!hasPair && cards[i].Rank == cards[i + 1].Rank)
                    {
                        hasPair = true;
                        fullHouseCards.AddRange(cards.GetRange(i, 2));
                    }

                    if(hasPair && hasThreeOfAKind)
                    {
                        handWithFullHouse = new Hand(fullHouseCards, HandRank.FullHouse, fullHouseCards);
                        break;
                    }
                }
            }
            return handWithFullHouse;
        }

        /// <summary>
        /// Tries to create a hand with a flush from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a flush.</param>
        /// <returns>Returns a hand with a flush if possible, otherwise null.</returns>
        public static Hand TryFlush(List<Card> cards)
        {
            Hand handWithFlush = null;
            if (cards != null && cards.Count >= 5)
            {
                Dictionary<CardSuit, List<Card>> cardListsBySuit = new Dictionary<CardSuit, List<Card>>();
                foreach (Card card in cards)
                {
                    if (!cardListsBySuit.ContainsKey(card.Suit))
                    {
                        cardListsBySuit.Add(card.Suit, new List<Card>());
                    }
                    // Uses the suit's enum value to add to the corresponding list
                    cardListsBySuit[card.Suit].Add(card);
                    if (cardListsBySuit[card.Suit].Count == 5)
                    {

                        List<Card> flushCards = cardListsBySuit[card.Suit];
                        handWithFlush = new Hand(flushCards, HandRank.Flush, flushCards);
                        // Breaks out of the loop because there is no need to continue
                        break;
                    }
                }
            }
            return handWithFlush;
        }

        /// <summary>
        /// Tries to create a hand with a straight from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a straight.</param>
        /// <returns>Returns a hand with a straight if possible, otherwise null.</returns>
        public static Hand TryStraight(List<Card> cards)
        {
            Hand handWithStraight = null;
            if (cards != null && cards.Count >= 5)
            {
                // Straight = five consecutive cards with correctly rising ranks
                List<Card> consecutiveCards = new List<Card>();
                // Starts with the first card as the beginning
                consecutiveCards.Add(cards[0]);
                int indexNextCard = 1;
                for (int i = 0; i < cards.Count - 1 && consecutiveCards.Count < 5; i++)
                {
                    Card currentCard = cards[i];
                    Card nextCard = cards[indexNextCard];
                    // If the next card is a rank lower than it is consecutive in the rank order
                    if (currentCard.Rank - 1 == nextCard.Rank)
                    {
                        consecutiveCards.Add(nextCard);
                    }
                    else if (currentCard.Rank != nextCard.Rank) // If the next card's rank is not one lower or equal then the cards are not consecutive
                    {
                        // Empties the list because the order was broken and has to start anew using the current card
                        consecutiveCards.Clear();
                        consecutiveCards.Add(nextCard);
                    }
                }
                // Special check for ace card, because rank-wise ace it is at the top order-wise it can be the bottom card (one/ace to five)
                if (consecutiveCards.Count == 4 && consecutiveCards[3].Rank == CardRank.Two && cards[0].Rank == CardRank.Ace)
                {
                    consecutiveCards.Add(cards[0]);
                }
                if (consecutiveCards.Count == 5)
                {
                    consecutiveCards = consecutiveCards.GetRange(0, 5);
                    handWithStraight = new Hand(consecutiveCards, HandRank.Straight, consecutiveCards);
                }
            }
            return handWithStraight;
        }

        /// <summary>
        /// Tries to create a hand with a three of a kind from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a three of a kind.</param>
        /// <returns>Returns a hand with a three of a kind followed up by the strongest leftover cards if possible, otherwise null.</returns>
        public static Hand TryThreeOfAKind(List<Card> cards)
        {
            Hand handWithThreeOfAKind = null;
            // Needs at least three cards to create a three of a kind
            if (cards != null && cards.Count >= 3)
            {
                // Sorts the cards so it tries finding a three of a kind using the stronger cards first
                cards = SortCardsByRank(cards);
                for (int i = 0; i < cards.Count - 2; i++)
                {
                    // Cards with the same rank should be next to each other because they were sorted by rank
                    if (cards[i].Rank == cards[i + 1].Rank && cards[i].Rank == cards[i + 2].Rank)
                    {
                        // If three matching cards were found then the requirements for a three of a kind are fulfilled
                        List<Card> threeOfAKind = new List<Card>() { cards[i], cards[i + 1], cards[i + 2] };
                        // Creates a list containing the remaining cards by removing the cards used for the three of a kind
                        List<Card> remainingCards = cards;
                        threeOfAKind.ForEach(card => remainingCards.Remove(card));
                        // Finishes the cards by adding the strongest remaining cards
                        List<Card> completedCards = CompleteCards(threeOfAKind, remainingCards);
                        handWithThreeOfAKind = new Hand(completedCards, HandRank.ThreeOfAKind, threeOfAKind);
                        // Breaks out of the loop because there is no need to continue
                        break;
                    }
                }
            }
            return handWithThreeOfAKind;
        }

        /// <summary>
        /// Tries to create a hand with two pairs from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for pairs.</param>
        /// <returns>Returns a hand with two pairs followed up by the strongest leftover card if possible, otherwise null.</returns>
        public static Hand TryTwoPair(List<Card> cards)
        {
            Hand handWithTwoPair = null;
            // Needs at least four cards to have two pairs
            if (cards != null && cards.Count >= 4)
            {
                List<Card> twoPair = new List<Card>();
                // Sorts the cards so it tries finding pairs using the stronger cards first
                cards = SortCardsByRank(cards);
                for (int i = 0; i < cards.Count - 1; i++)
                {
                    // Adds if the ranks match and no pair has been added (count is 0) or if the first pair is not the same rank as the second 
                    if (cards[i].Rank == cards[i + 1].Rank && (cards.Count == 0 || cards[0].Rank != cards[i].Rank))
                    {
                        twoPair.Add(cards[i]);
                        // ++i so it skips this card (in the for-loop) because it is added for the current pair
                        twoPair.Add(cards[++i]);
                        if (twoPair.Count == 4)
                        {
                            // Creates a list containing the remaining cards by removing the cards used for the two pair
                            List<Card> remainingCards = cards;
                            twoPair.ForEach(card => remainingCards.Remove(card));
                            // Finishes the cards by adding the strongest remaining cards
                            List<Card> completedCards = CompleteCards(twoPair, remainingCards);
                            handWithTwoPair = new Hand(completedCards, HandRank.TwoPair, twoPair);
                            // Breaks out of the loop because there is no need to continue
                            break;
                        }
                    }
                }
            }
            return handWithTwoPair;
        }

        /// <summary>
        /// Tries to create a hand with a pair from the given cards.
        /// </summary>
        /// <param name="cards">The cards checked for a pair.</param>
        /// <returns>Returns a hand with the strongest pair found followed up by the strongest leftover cards if possible, otherwise null.</returns>
        public static Hand TryPair(List<Card> cards)
        {
            Hand handWithOnePair = null;
            // Needs at least two cards to create a pair
            if (cards != null && cards.Count >= 2)
            {
                // Sorts the cards so it tries finding a pair using the stronger cards first
                cards = SortCardsByRank(cards);
                for (int i = 0; i < cards.Count - 1; i++)
                {
                    // Cards with the same rank should be next to each other because they were sorted by rank
                    if (cards[i].Rank == cards[i + 1].Rank)
                    {
                        // If a matching card was found then the requirements for a pair are fulfilled
                        List<Card> onePair = new List<Card>() { cards[i], cards[i + 1] };
                        // Creates a list containing the remaining cards by removing the cards used for the pair
                        List<Card> remainingCards = cards;
                        onePair.ForEach(card => remainingCards.Remove(card));
                        // Finishes the cards by adding the strongest remaining cards
                        List<Card> completedCards = CompleteCards(onePair, remainingCards);
                        handWithOnePair = new Hand(completedCards, HandRank.OnePair, onePair);
                        // Breaks out of the loop because there is no need to continue
                        break;
                    }
                }
            }
            return handWithOnePair;
        }

        #endregion

        #region private methods

        private static List<Card> SortCardsByRank(List<Card> unsortedCards)
        {
            List<Card> sortedCards = new List<Card>();
            // Needs to have the same amount of cards at the end
            while (sortedCards.Count != unsortedCards.Count)
            {
                Card strongestCard = null;
                for (int i = 0; i < unsortedCards.Count; i++)
                {
                    // Adds the strongest card found which hasn't been added yet
                    Card currentCard = unsortedCards[i];
                    if (!sortedCards.Contains(currentCard) && (strongestCard == null || currentCard.Rank > strongestCard.Rank))
                    {
                        strongestCard = currentCard;
                    }
                }
                sortedCards.Add(strongestCard);
            }
            // Returns the same if it is null or if there is only one card
            return sortedCards;
        }

        private static List<Card> CompleteCards(List<Card> usedCards, List<Card> remainingCards, int maxCards = MAX_CARDS_FOR_HAND)
        {
            if (usedCards != null && remainingCards != null)
            {
                // Sorts the remaining cards so that the stronger cards come first
                remainingCards = SortCardsByRank(remainingCards);
                for (int i = 0; i < remainingCards.Count && usedCards.Count < maxCards; i++)
                {
                    usedCards.Add(remainingCards[i]);
                }
            }
            return usedCards;
        }

        #endregion
    }
}
