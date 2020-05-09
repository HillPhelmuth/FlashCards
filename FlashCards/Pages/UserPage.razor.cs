using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Shared;

namespace FlashCards.Pages
{
    public class UserPageModel : FlashCardComponentBase
    {
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
            await UpdateState();
            DeckState.OnChange += UpdateState;
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
            deckName = newDeck.Name;
            await DeckState.UpdateSelectedDeckAsync(SelectedDeck, true);
            
            isAddCard = true;
            isSelectDeck = true;
        }
        protected async Task SelectDeck()
        {
            SelectedDeck = UserDecks.Find(x => x.Name == deckName);
            await DeckState.UpdateSelectedDeckAsync(SelectedDeck);
            isAddCard = true;
            DeckCards = DeckState.SelectedDeck?.Cards;
            
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
                var cards = await DeckState.GetDeckCards(deck);
                await DeckState.RemoveDeck(deck, cards);
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
