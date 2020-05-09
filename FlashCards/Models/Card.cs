using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Models
{
    [Serializable]
    [Table("Cards")]
    public class Card : CardDeckBase
    {
        public int ID { get; set; }
        public int Decks_ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [NotMapped]
        public List<AnswerData> DisplayAnswers { get; set; }
    }
}
