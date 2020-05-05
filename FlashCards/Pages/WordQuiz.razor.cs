using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Services;
using FlashCards.Interfaces;
using FlashCards.Models;
using FlashCards.Shared;
using System.Threading;
using System.Diagnostics;

namespace FlashCards.Pages
{
    public class WordQuizModel : FlashCardComponentBase
    {
        [Inject]
        protected IWordQuizService WordQuiz { get; set; }
        protected string QuizArea { get; set; }
        protected int QuizLevel { get; set; }
        protected WordQuizData QuizData { get; set; }
        protected QuizData CurrentQuiz { get; set; }
        protected List<QuizData> Quizzes { get; set; }
        //protected Stopwatch stopWatch;
        protected string timesUpMessage;
        protected bool isStartTime;
        protected bool isStopTime;
        protected int correctCount;
        protected int incorrectCount;
        protected int seconds;
        protected bool isQuizReady;
        protected bool isCorrect;
        protected bool isZoomDown;
        protected string answerMessage;
        protected string gameoverMessage;
        private int track;

        protected async Task GetWordQuiz()
        {
            if (QuizArea == null || QuizLevel == 0)
                return;

            QuizData = await WordQuiz.GetWordQuiz(QuizArea, QuizLevel);
            Quizzes = QuizData.Quizlist;
            CurrentQuiz = Quizzes[track];
            isQuizReady = true;
            isZoomDown = true;
            seconds = 10;
            isStartTime = true;

            StateHasChanged();
        }
        protected async Task EvaluateAnswer(int answer)
        {
            if (CurrentQuiz.Correct == answer)
            {
                isCorrect = true;
                answerMessage = "Correct!";
                correctCount++;
            }
            else
            {
                isCorrect = false;
                answerMessage = "Yous dont do Words good";
                incorrectCount++;
            }
            if (Quizzes.Count <= track)
            {
                gameoverMessage = $"Thats It! You answered {correctCount} correctly";
                isQuizReady = false;
                StateHasChanged();
                return;
            }
            isStartTime = false;
            seconds = 0;
            StateHasChanged();
            await GetNextQuestion();
        }
        protected Task GetNextQuestion()
        {
            track++;
            CurrentQuiz = Quizzes[track];
            isZoomDown = !isZoomDown;
            timesUpMessage = "";
            seconds = 10;
            isStartTime = true;
            StateHasChanged();
            return Task.CompletedTask;
        }
        protected void EnterTop() => isZoomDown = !isZoomDown;

        protected Task TimeIsUp(bool isup)
        {
            timesUpMessage = "Time's up!";
            isStartTime = !isStartTime;
            isStopTime = isup;
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}


