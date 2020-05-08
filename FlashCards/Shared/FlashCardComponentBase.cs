using FlashCards.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;

namespace FlashCards.Shared
{
    public abstract class FlashCardComponentBase : ComponentBase
    {
        [Inject]
        public FlashCardsDbService Database { get; set; }

        [Inject]
        protected DeckStateService DeckState { get; set; }
        
        public Deck SelectedDeck { get; set; }
        public List<Card> DeckCards { get; set; }

        protected async Task UpdateState()
        {
            SelectedDeck = DeckState.SelectedDeck;
            DeckCards = await DeckState.GetDeckCards(SelectedDeck);
            StateHasChanged();
        }
    }
}
