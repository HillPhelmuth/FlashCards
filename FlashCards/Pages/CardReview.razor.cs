using FlashCards.Extensions;
using FlashCards.Models;
using FlashCards.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Pages
{
    public class CardReviewModel : ComponentBase
    {
        [Inject]
        protected FlashCardsDbService Database { get; set; }
        [CascadingParameter]
        protected Deck SelectedDeck { get; set; }
        [CascadingParameter]
        protected List<Card> Cards { get; set; }
        protected Card DisplayCard { get; set; }
        protected List<AnswerData> Answers { get; set; }

        protected int trackNext = 1;
        protected int correctTotal;
        protected int wrontTotal;
        protected string message;
        protected bool enabled;
        protected bool isReady;

        protected override Task OnInitializedAsync()
        {
            //Cards = await Database.GetDeckCards(SelectedDeck);
            Cards = Cards.AddAltAnswers();
            Cards.Shuffle();
            isReady = true;
            enabled = true;
            DisplayCard = Cards[0];
            //trackNext++;
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
                StateHasChanged();
                return;
            }
            answer.IsCorrect = false;
            answer.IsIncorrect = true;
            answer.CssClass = "wrong";
            wrontTotal++;
            StateHasChanged();
        }
        protected void CardAnimEnd() => enabled = false;
        protected void WrongAnimEnd(AnswerData answer) => answer.IsIncorrect = !answer.IsIncorrect;
        protected void CorrectAnimEnd(AnswerData answer) => answer.IsCorrect = !answer.IsCorrect;
    }
}
