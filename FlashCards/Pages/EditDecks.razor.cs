using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Shared;

namespace FlashCards.Pages
{
    public class EditDecksModel : FlashCardComponentBase, IDisposable
    {
        public List<Card> DisplayCards { get; set; }
        protected Card cardEdit;
        protected Deck deckEdit;
        protected bool showCards;
        protected bool showEdit;
        protected string question;
        protected string answer;

        protected override async Task OnInitializedAsync()
        {
            await UpdateState();
            DeckState.OnChange += UpdateState;
        }

        protected async Task ShowDeckCards(Deck deck)
        {
            deckEdit = deck;
            DisplayCards = await DeckState.GetDeckCards(deck);
            showCards = true;
            StateHasChanged();
        }

        protected Task EditCard(Card card)
        {
            cardEdit = card;
            question = cardEdit.Question;
            answer = cardEdit.Answer;
            showEdit = true;
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected async Task UpdateCard()
        {
            cardEdit.Answer = answer;
            cardEdit.Question = question;
            await DeckState.UpdateDeckCard(deckEdit, cardEdit);
            showEdit = false;
            StateHasChanged();
        }
        public void Dispose() => DeckState.OnChange -= UpdateState;
    }
}
