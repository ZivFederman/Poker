using System.Collections.Generic;

namespace TexasHoldemLib
{
    public class Room
    {
        public CardDealer Dealer { get; set; }
        public List<Player> Players { get; set; }
    }
}
