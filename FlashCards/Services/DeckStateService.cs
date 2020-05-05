using System;
using System.Collections.Generic;
using FlashCards.Models;

namespace FlashCards.Services
{
    public class DeckStateService
    {
        public FlashCardStats DeckStats { get; private set; }
        public Deck Deck { get; private set; }
        public List<Card> Cards { get; private set; }

        public event Action OnChange;

        public void UpdateSelectedDeck(Deck deck)
        {
            DeckStats = new FlashCardStats()
            {
                Deck = deck
            };
            Deck = deck;
            NotifyStateChanged();
        }
        public void UpdateStats(bool isCorrect)
        {
            if (isCorrect)
                DeckStats.Correct++;
            else
                DeckStats.Wrong++;
            var correct = DeckStats.Correct;
            var total = DeckStats.Correct + DeckStats.Wrong;
            DeckStats.TotalPct = correct / total;
            NotifyStateChanged();
        }
        public void UpdateDeckCards(Deck deck, List<Card> cards)
        {
            Deck = deck;
            Cards = cards;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
