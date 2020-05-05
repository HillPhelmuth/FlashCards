using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class VocabAnswer : AnswerData
    {
        public string PartOfSpeech { get; set; }
        public List<string> Synonyms { get; set; }
        public List<string> TypeOfList { get; set; }
    }
}
