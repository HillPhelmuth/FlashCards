using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class StatsBase
    {
        public string UserId { get; set; }
        public decimal Correct { get; set; }
        public decimal Wrong { get; set; }
        public decimal TotalPct { get; set; }
    }
}
