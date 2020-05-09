using FlashCards.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Data
{
    public class FlashCardsDbContext : DbContext
    {
        public FlashCardsDbContext(DbContextOptions<FlashCardsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Deck> DecksTable { get; set; }
        public DbSet<Card> CardsTable { get; set; }
        public DbSet<DeckStats> StatsTable { get; set; }
    }
}
