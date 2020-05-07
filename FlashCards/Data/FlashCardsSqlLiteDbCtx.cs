using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Data
{
    public class FlashCardsSqlLiteDbCtx : DbContext
    {
        public FlashCardsSqlLiteDbCtx(DbContextOptions<FlashCardsSqlLiteDbCtx> options)
            : base(options)
        {

        }
        public DbSet<Deck> DecksTable { get; set; }
        public DbSet<Card> CardsTable { get; set; }
        public DbSet<DeckStats> StatsTable { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=flashcards.db");
    }
}
