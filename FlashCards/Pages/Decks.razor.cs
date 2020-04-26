using FlashCards.Models;
using FlashCards.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Pages
{
    public class DecksModel : ComponentBase
    {
        [Inject]
        protected FlashCardsDbService Database { get; set; }
        [Inject]
        protected WordsApiService WordsApi { get; set; }

        [CascadingParameter]
        protected Deck selectedDeck { get; set; }
        [CascadingParameter]
        protected List<Deck> UserDecks { get; set; }
        [Parameter]
        public List<Card> UserCards { get; set; }
        [Parameter]
        public EventCallback<List<Card>> UserCardsChanged { get; set; }

        protected string question;
        protected string answer;
        protected string wordSearch;
        protected string toggleLabel = "Standard Card";
        protected string confirmMessage = "";
        protected string confirmCss = "";
        protected bool isVocab = false;
        protected bool deleteConfirm = false;
        protected bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            UserCards = await Database.GetDeckCards(selectedDeck);
            if (selectedDeck.Cards == null)
                selectedDeck.Cards = new List<Card>();
            selectedDeck.Cards.Distinct().ToList().AddRange(UserCards);
            //await UserCardsChanged.InvokeAsync(UserCards);
        }
        protected void ToggleVocab()
        {
            toggleLabel = toggleLabel == "Standard Card" ? "Vocabulary Card" : "Standard Card";
        }

        protected async Task AddCardToDeck()
        {
            if (selectedDeck.Cards == null)
                selectedDeck.Cards = new List<Card>();
            var newCard = new Card() { Question = question, Answer = answer };
            selectedDeck.Cards.Add(newCard);
            await Database.AddCardToDeck(newCard, selectedDeck);
            question = null;
            answer = null;

            await UserCardsChanged.InvokeAsync(selectedDeck.Cards);
            StateHasChanged();
        }
        protected async Task CreateVocabCard()
        {
            if (wordSearch == null)
                return;
            var definition = await WordsApi.GetDefinitions(wordSearch);
            var firstDefinition = definition?.Definitions?.FirstOrDefault() ?? new DefinitionData() { Definition = "NO DEFINITONA FOUND" };
            var newCard = new Card()
            {
                Question = wordSearch,
                Answer = $"Definition: {firstDefinition.Definition}"
            };
            (selectedDeck.Cards ?? (selectedDeck.Cards = new List<Card>())).Add(newCard);
            await Database.AddCardToDeck(newCard, selectedDeck);
            await UserCardsChanged.InvokeAsync(selectedDeck.Cards);
            wordSearch = null;
            StateHasChanged();
        }
        protected async Task CreateRandomVocabCards(bool isMany = true)
        {

            
            var definitions = await WordsApi.GetDefinitions(isMany);
            foreach (var definition in definitions)
            {
                var firstDefinition = definition?.RandomDefinitions?.FirstOrDefault() ?? new DefinitionData() { Definition = "NO DEFINITION FOUND" };
                if (firstDefinition.Definition != "NO DEFINITION FOUND")
                {
                    var newCard = new Card()
                    {
                        Question = definition.Word,
                        Answer = $"<strong>Definition:</strong> {firstDefinition.Definition}"
                    };
                    (selectedDeck.Cards ?? (selectedDeck.Cards = new List<Card>())).Add(newCard);
                    await Database.AddCardToDeck(newCard, selectedDeck);
                }
            }
            await UserCardsChanged.InvokeAsync(selectedDeck.Cards);
            wordSearch = null;
            isLoading = false;
            StateHasChanged();
        }
        protected void ShowLoading(MouseEventArgs args)
        {
            isLoading = true;
            StateHasChanged();
        }

        protected async Task VocakKeyUp(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await CreateVocabCard();
            }
        }
        protected async Task DeleteCard(Card card)
        {
            if (card.IsDeleteConfirm)
            {
                await Database.RemoveCardFromDeck(card);
                selectedDeck.Cards.Remove(card);
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
    }
}
