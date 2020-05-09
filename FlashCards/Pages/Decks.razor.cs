using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Shared;

namespace FlashCards.Pages
{
    public class DecksModel : FlashCardComponentBase, IDisposable
    {
        protected string question;
        protected string answer;
        
        protected bool isShowCards;
        protected override async Task OnInitializedAsync()
        {
            await UpdateState();
            DeckState.OnChange += UpdateState;
            SelectedDeck.Cards ??= new List<Card>();
            DeckCards ??= new List<Card>();
            SelectedDeck.Cards.Distinct().ToList().AddRange(DeckCards);
        }

        protected async Task AddCardToDeck()
        {
            SelectedDeck.Cards ??= new List<Card>();
            var newCard = new Card() { Question = question, Answer = answer };
            SelectedDeck.Cards.Add(newCard);
            await DeckState.AddCardToDeck(newCard, SelectedDeck);
            question = null;
            answer = null;
            DeckCards = SelectedDeck.Cards;
            await DeckState.UpdateDeckCards(SelectedDeck, DeckCards);
            StateHasChanged();
        }
        protected async Task DeleteCard(Card card)
        {
            if (card.IsDeleteConfirm)
            {
                await DeckState.RemoveCardFromDeck(card);
                SelectedDeck.Cards.Remove(card);
                await DeckState.UpdateDeckCards(SelectedDeck, SelectedDeck.Cards);
                card.ConfirmDelete = "";
                card.CssConfirmClass = "";
            }
            else
            {
                card.ConfirmDelete = "Delete forever?";
                card.CssConfirmClass = "wrong";
            }
            card.IsDeleteConfirm = !card.IsDeleteConfirm;
            StateHasChanged();
        }
        public void Dispose() => DeckState.OnChange -= UpdateState;
    }
}
