using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlashCards.Models;

namespace FlashCards.Services
{
    public class DeckStateService
    {
        private readonly FlashCardsDbService _dbService;
        public DeckStats DeckStats { get; private set; }
        public Deck SelectedDeck { get; private set; }
        public List<Card> Cards { get; private set; }
        public List<Deck> UserDecks { get; private set; }

        public event Func<Task> OnChange;

        public DeckStateService(FlashCardsDbService dbService)
        {
            _dbService = dbService;
        }
        public async Task UpdateSelectedDeckAsync(Deck deck)
        {
            DeckStats = new DeckStats()
            {
                Deck = deck
            };
            SelectedDeck = deck;
            await NotifyStateChanged();
        }

        public async Task UpdateUserDeckCards(List<Deck> decks)
        {
            foreach (var deck in decks)
            {
                var cards = await _dbService.GetDeckCards(deck);
                deck.Cards = cards;
            }
            UserDecks = decks;
            await NotifyStateChanged();
        }
        public async Task UpdateStats(bool isCorrect)
        {
            if (isCorrect)
                DeckStats.Correct++;
            else
                DeckStats.InCorrect++;
            var correct = DeckStats.Correct;
            var total = DeckStats.Correct + DeckStats.InCorrect;
            DeckStats.TotalPct = correct / total;
            await NotifyStateChanged();
        }
        public async Task UpdateDeckCards(Deck deck, List<Card> cards)
        {
            SelectedDeck = deck;
            Cards = cards;
            SelectedDeck.Cards = cards;
            await NotifyStateChanged();
        }

        public async Task<List<Card>> GetDeckCards(Deck deck)
        {
            return await _dbService.GetDeckCards(deck);
        }
        private async Task NotifyStateChanged()
        {
            if (OnChange != null) await OnChange?.Invoke();
        }
    }
}
