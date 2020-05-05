using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Services;
using Microsoft.AspNetCore.Components;

namespace FlashCards.Pages
{
    public class UserPageModel : ComponentBase
    {
        [Inject]
        protected FlashCardsDbService Database { get; set; }
        [Inject]
        protected DeckStateService DeckState { get; set; }
        [Parameter]
        public List<Deck> UserDecks { get; set; }
        [Parameter]
        public Deck SelectedDeck { get; set; }
        [Parameter]
        public List<Card> DeckCards { get; set; }
        protected Deck newDeck = new Deck();
        protected string deckName = "no deck selected";
        protected string deckDeleteMessage;
        protected bool userHasDecks;
        protected bool addNewToggle;
        protected bool isAddCard;
        protected bool isSelectDeck;
        protected bool isReview;
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
            UserDecks ??= new List<Deck>();
            UserDecks.Add(newDeck);
            SelectedDeck = newDeck;
            DeckState.UpdateSelectedDeck(SelectedDeck);
            await Database.AddDeck(newDeck.Name, newDeck.Subject);
            isAddCard = true;
            isSelectDeck = true;
        }
        protected async Task SelectDeck()
        {
            SelectedDeck = UserDecks.Find(x => x.Name == deckName);
            DeckState.UpdateSelectedDeck(SelectedDeck);
            isAddCard = true;
            DeckCards = await Database.GetDeckCards(SelectedDeck);
            if (SelectedDeck != null) SelectedDeck.Cards = DeckCards;
            DeckState.UpdateDeckCards(SelectedDeck, DeckCards);
            isSelectDeck = true;
            StateHasChanged();
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
       
    }
}
