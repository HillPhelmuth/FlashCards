using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Models
{
    public class StatsBase
    {
        [Column(TypeName = "decimal(6,3)")]
        public decimal Correct { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal InCorrect { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal TotalPct { get; set; }
    }
}
