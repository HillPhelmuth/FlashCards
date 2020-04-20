using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    [Serializable]
    [Table("Cards")]
    public class Card
    {
        public int ID { get; set; }
        public int Decks_ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [NotMapped]
        public List<string> AltAnswers { get; set; }
        
    }
}
