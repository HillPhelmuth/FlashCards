using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class CardDeckBase
    {
        [NotMapped]
        public string ConfirmDelete { get; set; }
        [NotMapped]
        public bool IsDeleteConfirm { get; set; }
        [NotMapped]
        public string CssConfirmClass { get; set; }
    }
}
