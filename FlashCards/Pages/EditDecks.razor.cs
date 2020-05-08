using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Shared;
using Microsoft.AspNetCore.Components;

namespace FlashCards.Pages
{
    public class EditDecksModel : FlashCardComponentBase
    {
        protected List<Deck> UserDecks { get; set; }
        [Parameter]
        public List<Card> DisplayCards { get; set; }
        protected Card cardEdit;
        protected Deck deckEdit;
        protected bool showCards;
        protected bool showEdit;
        protected string question;
        protected string answer;
        protected string[] cardTempArray;
       

        protected override async Task OnInitializedAsync()
        {
            var decks = await Database.GetUserDecks();
            await DeckState.UpdateUserDeckCards(decks);
            UserDecks = DeckState.UserDecks;

            //DeckState.OnChange += UpdateState;
        }

        protected async Task ShowDeckCards(Deck deck)
        {
            deckEdit = deck;
            DisplayCards = await Database.GetDeckCards(deck);
            cardTempArray = DisplayCards.Select(x => x.Question).ToArray();
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
            await Database.UpdateDeckCards(cardEdit, deckEdit);
            showEdit = false;
            StateHasChanged();
        }
        //public void Dispose() => DeckState.OnChange -= UpdateState;
    }
}
