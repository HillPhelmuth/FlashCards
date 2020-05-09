using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;

namespace FlashCards.Services
{
    public class DeckStateService
    {
        private readonly FlashCardsDbService Database;
        public DeckStats DeckStats { get; private set; }
        public Deck SelectedDeck { get; private set; }
        public List<Card> Cards { get; private set; }
        public List<Deck> UserDecks { get; private set; }

        public event Func<Task> OnChange;

        public DeckStateService(FlashCardsDbService database)
        {
            Database = database;
        }

        public bool HasUser => Database.HasUser;
        public async Task UpdateSelectedDeckAsync(Deck deck, bool isNew = false)
        {
            DeckStats = await Database.GetDeckStats(deck);
            DeckStats ??= new DeckStats { Deck = deck };
            if (isNew)
                await Database.AddDeck(deck.Name, deck.Subject);
            SelectedDeck = deck;
            await NotifyStateChanged();
        }

        public async Task UpdateUserDeckCards(List<Deck> decks)
        {
            foreach (var deck in decks)
            {
                var cards = await Database.GetDeckCards(deck);
                deck.Cards = cards;
            }
            UserDecks = decks.Distinct().ToList();
            await NotifyStateChanged();
        }

        public async Task UpdateDeckCard(Deck deck, Card card)
        {
            await Database.UpdateDeckCards(card, deck);
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
            await Database.UpdateStats(DeckStats, SelectedDeck);
            await NotifyStateChanged();
        }
        public async Task ResetDeckStats(Deck deck)
        {
            DeckStats = new DeckStats
            {
                Deck = deck,
                Correct = 0,
                InCorrect = 0,
                TotalPct = 0
            };
            await Database.UpdateStats(DeckStats, SelectedDeck);
            await NotifyStateChanged();
        }
        public async Task<DeckStats> GetDeckStats(Deck deck)
        {
            return await Database.GetDeckStats(deck);
        }
        public async Task UpdateDeckCards(Deck deck, List<Card> cards)
        {
            SelectedDeck = deck;
            Cards = cards;
            SelectedDeck.Cards = cards;
            await NotifyStateChanged();
        }

        public async Task RemoveDeck(Deck deck, List<Card> cards)
        {
            await Database.RemoveDeck(deck, cards);
            await NotifyStateChanged();
        }

        public async Task RemoveCardFromDeck(Card card)
        {
            await Database.RemoveCardFromDeck(card);
            Cards.Remove(card);
            SelectedDeck?.Cards.Remove(card);
            await NotifyStateChanged();
        }

        public async Task AddCardToDeck(Card card, Deck deck)
        {
            await Database.AddCardToDeck(card, deck);
        }

        public async Task<List<Deck>> GetUserDecks()
        {
            var userDecks = await Database.GetUserDecks();
            await UpdateUserDeckCards(userDecks);
            return userDecks.Distinct().ToList();
        }
        public async Task<List<Card>> GetDeckCards(Deck deck)
        {
            return await Database.GetDeckCards(deck);
        }

        private async Task NotifyStateChanged()
        {
            if (OnChange != null) await OnChange.Invoke();
        }
    }
}
