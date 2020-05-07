using FlashCards.Extensions;
using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Shared;

namespace FlashCards.Pages
{
    public class CardReviewModel : FlashCardComponentBase, IDisposable
    {
        //[Inject]
        //protected FlashCardsDbService Database { get; set; }
        //[Inject]
        //protected DeckStateService DeckState { get; set; }
        //[CascadingParameter]
        //protected Deck SelectedDeck { get; set; }
        //[CascadingParameter]
        protected List<Card> Cards { get; set; }
        protected Card DisplayCard { get; set; }
        protected List<AnswerData> Answers { get; set; }

        protected int trackNext = 1;
        protected int correctTotal;
        protected int wrongTotal;
        protected string message;
        protected bool enabled;
        protected bool isReady;

        protected override Task OnInitializedAsync()
        {
            DeckState.OnChange += StateHasChanged;
            Cards = DeckState.Cards;
            Cards = Cards.AddAltAnswers();
            Cards.Shuffle();
            isReady = true;
            enabled = true;
            DisplayCard = Cards[0];
            Answers = DisplayCard.DisplayAnswers;
            Answers.Shuffle();
            return base.OnInitializedAsync();
        }
        protected void GetNext()
        {
            if (Cards.Count <= trackNext)
            {
                return;
            }
            enabled = true;
            DisplayCard = Cards[trackNext];
            trackNext++;
            Answers = DisplayCard.DisplayAnswers;
            Answers.Shuffle();
            StateHasChanged();
        }
        protected void CheckAnswer(AnswerData answer)
        {
            if (answer.Answer == DisplayCard.Answer)
            {
                answer.IsCorrect = true;
                answer.IsIncorrect = false;
                answer.CssClass = "correct";
                correctTotal++;
                DeckState.UpdateStats(true);
                StateHasChanged();
                return;
            }
            answer.IsCorrect = false;
            answer.IsIncorrect = true;
            answer.CssClass = "wrong";
            wrongTotal++;
            DeckState.UpdateStats(false);
            StateHasChanged();
        }
        protected void CardAnimEnd() => enabled = false;
        //protected void WrongAnimEnd(AnswerData answer) => answer.IsIncorrect = !answer.IsIncorrect;
        //protected void CorrectAnimEnd(AnswerData answer) => answer.IsCorrect = !answer.IsCorrect;

        public void Dispose() => DeckState.OnChange -= StateHasChanged;
    }
}
