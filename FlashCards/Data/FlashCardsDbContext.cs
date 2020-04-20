using FlashCards.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
