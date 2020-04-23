using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class AnswerData
    {
        public string Answer { get; set; }
        public bool IsIncorrect { get; set; }
        public bool IsCorrect { get; set; }
        public string CssClass { get; set; }
    }
}
