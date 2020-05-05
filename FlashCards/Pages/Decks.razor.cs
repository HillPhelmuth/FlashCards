using FlashCards.Interfaces;
using FlashCards.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Shared;

namespace FlashCards.Pages
{
    public class DecksModel : FlashCardComponentBase, IDisposable
    {
        [Inject]
        protected IWordsApiService WordsApi { get; set; }

        protected Deck SelectedDeck { get; set; }

        public List<Card> DeckCards { get; set; }

        protected string question;
        protected string answer;
        protected string wordSearch;
        protected bool isLoading;
        protected bool isShowCards;
        protected override async Task OnInitializedAsync()
        {
            SelectedDeck = DeckState.Deck;
            DeckCards = await Database.GetDeckCards(SelectedDeck);
            DeckState.OnChange += StateHasChanged;
            SelectedDeck.Cards ??= new List<Card>();
            SelectedDeck.Cards.Distinct().ToList().AddRange(DeckCards);
        }

        protected async Task AddCardToDeck()
        {
            SelectedDeck.Cards ??= new List<Card>();
            var newCard = new Card() { Question = question, Answer = answer };
            SelectedDeck.Cards.Add(newCard);
            await Database.AddCardToDeck(newCard, SelectedDeck);
            question = null;
            answer = null;
            DeckState.UpdateDeckCards(SelectedDeck, DeckCards);
            StateHasChanged();
        }
        protected async Task CreateVocabCard()
        {
            if (wordSearch == null)
                return;
            var definition = await WordsApi.GetDefinitions(wordSearch);
            var firstDefinition = definition?.Definitions?.FirstOrDefault() ?? new DefinitionData() { Definition = "NO DEFINITION FOUND" };
            await CreateVocabCard(definition, firstDefinition);
            DeckState.UpdateDeckCards(SelectedDeck, DeckCards);
            wordSearch = null;
            StateHasChanged();
        }
        protected async Task CreateRandomVocabCards(bool isMany = true)
        {
            var definitions = await WordsApi.GetDefinitions(isMany);
            foreach (var definition in definitions)
            {
                var firstDefinition = definition?.RandomDefinitions?.FirstOrDefault() ?? new DefinitionData() { Definition = "NO DEFINITION FOUND" };
                await CreateVocabCard(definition, firstDefinition);
            }
            DeckState.UpdateDeckCards(SelectedDeck, DeckCards);
            wordSearch = null;
            isLoading = false;
            StateHasChanged();
        }

        private async Task CreateVocabCard(DefinitionModel definition, DefinitionData firstDefinition)
        {
            if (firstDefinition.Definition != "NO DEFINITION FOUND")
            {
                var newCard = new Card()
                {
                    Question = definition.Word,
                    Answer = $"Definition -- {firstDefinition.Definition}"
                };
                (SelectedDeck.Cards ?? (SelectedDeck.Cards = new List<Card>())).Add(newCard);
                await Database.AddCardToDeck(newCard, SelectedDeck);
            }
        }

        protected void ShowLoading(MouseEventArgs args)
        {
            isLoading = true;
            StateHasChanged();
        }

        protected async Task VocabKeyUp(KeyboardEventArgs args)
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
                SelectedDeck.Cards.Remove(card);
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
        public void Dispose() => DeckState.OnChange -= StateHasChanged;
    }
}
