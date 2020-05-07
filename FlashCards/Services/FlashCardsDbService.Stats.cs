using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Services
{
    public partial class FlashCardsDbService
    {
        [HttpPost]
        public async Task AddStats(DeckStats stats, Deck deck)
        {
            if (stats.ID > 0)
                return;
            var context = _context;

            // Match name and user_id to find stored DecksTable primary key
            stats.Decks_ID = context.DecksTable
                .Where(x => x.User_ID == UserId && x.Name == deck.Name)
                .Select(x => x.ID).FirstOrDefault();

            await context.AddAsync(stats);
            await context.SaveChangesAsync();

        }

        [HttpPut]
        public async Task UpdateStats(DeckStats stats, Deck deck)
        {
            var context = _context;
            var newCorrect = stats.Correct;
            var newInCorrect = stats.InCorrect;
            var newTotalPct = stats.TotalPct;
            var oldStats = await context.StatsTable.FirstOrDefaultAsync(x => x.Decks_ID == deck.ID);
            if (oldStats != null)
            {
                oldStats.Correct = newCorrect;
                oldStats.InCorrect = newInCorrect;
                oldStats.TotalPct = newTotalPct;
            }

            await context.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task DeleteStats(DeckStats stats)
        {
            var context = _context;
            context.Attach(stats);
            context.Remove(stats);
            await context.SaveChangesAsync();
        }
    }
}
