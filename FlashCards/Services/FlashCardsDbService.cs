using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Services
{
    public class FlashCardsDbService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private readonly FlashCardsDbContext _context;
        public FlashCardsDbService(AuthenticationStateProvider authenticationStateProvider, FlashCardsDbContext context)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
        }
        private string UserId
        {
            get
            {
                var authState = _authenticationStateProvider.GetAuthenticationStateAsync();
                return authState.Result.User.Identity.Name;
            }
        }
        public bool HasUser => !string.IsNullOrEmpty(UserId);
        [HttpPost]
        public async Task AddDeck(string deckname, string subject)
        {
            var userId = UserId;
            var playlist = new Deck() { Name = deckname, User_ID = userId, Subject = subject };
            var context = _context;
            var userPlaylist = context.DecksTable.ToList().Where(x => x.Name == deckname && x.User_ID == userId).FirstOrDefault();
            if (userPlaylist != playlist)
            {
                await context.AddAsync(playlist);
                await context.SaveChangesAsync();
            }
        }
        [HttpGet]
        public async Task<List<Deck>> GetUserDecks()
        {
            var userId = UserId;
            var context = _context;
            return await context.DecksTable.Where(x => x.User_ID == userId).ToListAsync();
        }
        [HttpGet]
        public async Task<Deck> GetDeckWithKey(Deck deck)
        {
            var userId = UserId;
            var context = _context;
            var playlists = await context.DecksTable.Where(x => x.User_ID == userId).ToListAsync();
            return playlists.Find(x => x.Name == deck.Name);
        }
        [HttpPost]
        public async Task AddCardToDeck(Card card, Deck deck)
        {
            if (card.ID > 0)
                return;
            var context = _context;
            // Match name and user_id to find stored DecksTable primary key
            card.Decks_ID = context.DecksTable
                .Where(x => x.User_ID == UserId && x.Name == deck.Name)
                .Select(x => x.ID).FirstOrDefault();
            await context.AddAsync(card);
            await context.SaveChangesAsync();
        }
        [HttpGet]
        public async Task<List<Card>> GetDeckCards(Deck deck)
        {
            var context = _context;
            var deckId = deck.ID;
            return await context.CardsTable.Where(x => x.Decks_ID == deckId).ToListAsync();
        }
    }
}
