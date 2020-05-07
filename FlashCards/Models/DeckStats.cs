using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class DeckStats : StatsBase
    {
        public int ID { get; set; }
        public int Decks_ID { get; set; }

        [NotMapped]
        public Deck Deck { get; set; }
    }
}
