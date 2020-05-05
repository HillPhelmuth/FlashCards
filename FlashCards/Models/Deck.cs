using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    [Serializable]
    [Table("Decks")]
    public class Deck : CardDeckBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string User_ID { get; set; }
        [NotMapped]
        public List<Card> Cards { get; set; }
    }
}
