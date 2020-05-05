using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class FlashCardStats : StatsBase
    {
        public Deck Deck { get; set; }
        //public string DeckName { get; set; }
        //public string DeckSubject { get; set; }
    }
}
