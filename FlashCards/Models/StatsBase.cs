using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class StatsBase
    {
        public decimal Correct { get; set; }
        public decimal InCorrect { get; set; }
        public decimal TotalPct { get; set; }
    }
}
