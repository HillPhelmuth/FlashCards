using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Services;
using FlashCards.Models;
using FlashCards.Extensions;

namespace FlashCards.Pages
{
    public class UserPageModel : ComponentBase
    {
        [Inject]
        protected FlashCardsDbService Database { get; set; }
        [Parameter]
        public List<Deck> UserDecks { get; set; }
        [Parameter]
        public Deck selectedDeck { get; set; }
        [Parameter]
        public List<Card> deckCards { get; set; }
        protected Deck newDeck = new Deck();
        protected string deckName = "no deck selected";
        protected string deckDeleteMessage;
        protected bool userHasDecks = false;
        protected bool addNewToggle = false;
        protected bool isAddCard = false;
        protected bool isSelectDeck = false;
        protected bool isReview = false;
        protected bool panelOpenState;
        protected override async Task OnInitializedAsync()
        {
            UserDecks = await Database.GetUserDecks();
            if (UserDecks.Count > 0)
                userHasDecks = true;
            else
                addNewToggle = true;
        }
        protected async Task AddDeck()
        {
            if (UserDecks == null)
                UserDecks = new List<Deck>();
            UserDecks.Add(newDeck);
            selectedDeck = newDeck;

            await Database.AddDeck(newDeck.Name, newDeck.Subject);
            isAddCard = true;
            isSelectDeck = true;
        }
        protected async Task SelectDeck()
        {
            selectedDeck = UserDecks.Where(x => x.Name == deckName).FirstOrDefault();
            isAddCard = true;
            deckCards = await Database.GetDeckCards(selectedDeck);
            selectedDeck.Cards = deckCards;
            isSelectDeck = true;
            StateHasChanged();
        }
        protected async Task ReviewCards()
        {
            if (selectedDeck == null)
                return;
            deckCards = await Database.GetDeckCards(selectedDeck);
            isAddCard = true;
            isSelectDeck = false;
            isReview = true;

        }
        protected void Reset()
        {
            userHasDecks = false;
            addNewToggle = false;
            isAddCard = false;
            isReview = false;
            isSelectDeck = false;
            deckName = "no deck selected";
            if (UserDecks.Count > 0)
                userHasDecks = true;
            else
                addNewToggle = true;
            StateHasChanged();
        }
        protected async Task DeleteDeck(Deck deck)
        {
            if (deck.IsDeleteConfirm)
            {
                var cards = await Database.GetDeckCards(deck);
                await Database.RemoveDeck(deck, cards);
                UserDecks.Remove(deck);
                deck.ConfirmDelete = "";
                deck.CssConfirmClass = "";
            }
            else
            {
                deck.ConfirmDelete = "Delete this deck and all card contents, forever?";
                deck.CssConfirmClass = "deckDelete zoom";
            }
            deck.IsDeleteConfirm = !deck.IsDeleteConfirm;
            deckDeleteMessage = deck.ConfirmDelete;
        }
        protected void CardsChangeHandler(List<Card> cards)
        {
            selectedDeck.Cards = cards.Distinct().ToList();
            deckCards = cards.Distinct().ToList();
            StateHasChanged();
        }
    }
}
