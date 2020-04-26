using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Services;
using FlashCards.Interfaces;
using FlashCards.Models;

namespace FlashCards.Pages
{
    public class WordQuizModel : ComponentBase
    {
        [Inject]
        protected IWordQuizService WordQuiz { get; set; }
        protected string QuizArea { get; set; }
        protected int QuizLevel { get; set; }
        protected WordQuizData QuizData { get; set; }
        protected QuizData CurrentQuiz { get; set; }
        protected List<QuizData> Quizzes { get; set; }

        protected int correctCount;
        protected int incorrectCount;
        protected bool isQuizReady = false;
        protected bool isCorrect;
        protected bool isZoomLeft;
        protected bool isZoomRight;
        protected bool isZoomDown;
        protected string answerMessage;
        protected string gameoverMessage;

        private int track = 0;

        protected async Task GetWordQuiz()
        {
            if (QuizArea == null || QuizLevel == 0)
                return;
            if (QuizArea == "es" && QuizLevel > 5)
                QuizLevel = 5;
            if (QuizArea == "ms" && QuizLevel > 7)
                QuizLevel = 7;
            if (QuizArea == "hs" && QuizLevel > 9)
                QuizLevel = 9;

            QuizData = await WordQuiz.GetWordQuiz(QuizArea, QuizLevel);
            Quizzes = QuizData.Quizlist;
            CurrentQuiz = Quizzes[track];
            track++;
            isQuizReady = true;
            isZoomDown = true;
            StateHasChanged();
        }
        protected async Task EvaluateAnswer(int answer)
        {
            if (CurrentQuiz.Correct == answer)
            {
                isCorrect = true;
                answerMessage = "Correct!";
                correctCount++;
                track++;
            }
            else
            {
                isCorrect = false;
                answerMessage = "Yous dont do Words good";
                incorrectCount++;
                track++;
            }
            if (Quizzes.Count <= track)
            {
                gameoverMessage = $"Thats It! You answered {correctCount} correctly";
                isQuizReady = false;
                StateHasChanged();
                return;
            }
            await Task.Run(() => CurrentQuiz = Quizzes[track]);
            StateHasChanged();
        }       
        protected void EnterTop() => isZoomDown = !isZoomDown;
    }
}
