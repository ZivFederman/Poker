// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using TexasHoldemLib;
List<Card> cards = new List<Card>()
{
    new Card(CardRank.Five, CardSuit.Spades),
    new Card(CardRank.Ace, CardSuit.Spades),
    new Card(CardRank.Four, CardSuit.Spades),
    new Card(CardRank.Two, CardSuit.Spades),
    new Card(CardRank.Three, CardSuit.Spades),
    new Card(CardRank.Jack, CardSuit.Hearts),
    new Card(CardRank.Eight, CardSuit.Diamonds)
};

string requestBody = JsonSerializer.Serialize(cards);

using (var client = new HttpClient())
{
    client.BaseAddress = new Uri("https://localhost:7051");

    // serialize your json using newtonsoft json serializer then add it to the StringContent
    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

    // method address would be like api/callUber:SomePort for example
    var result = await client.PostAsync("/HoldemHand/EvaluateHand", content);
    string resultContent = await result.Content.ReadAsStringAsync();
    Hand hand = JsonSerializer.Deserialize<Hand>(resultContent);
    Console.WriteLine("Hand: " + hand.Rank.ToString());
    hand.RankRelevantCards.ForEach(c => Console.WriteLine("Card: {0}, ({1})", c.Rank, c.Suit));
    Console.ReadKey();
}