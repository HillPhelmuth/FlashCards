using FlashCards.Interfaces;
using FlashCards.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FlashCards.Pages
{
    public class MathQuizModel : ComponentBase
    {
        [Inject]
        protected IMathQuizService MathQuizService { get; set; }
        protected string MathTopic { get; set; }
        protected string MathDifficulty { get; set; }

        protected string answerCss;
        protected string answerMessage;
        protected bool isSelectQuiz;
        protected bool isDisplayMessage;

        protected MathQuestionModel mathQuestion;

        protected async Task GetMathQuiz()
        {
            mathQuestion = await MathQuizService.GetMathQuestion(MathTopic, MathDifficulty);
            isSelectQuiz = true;
            StateHasChanged();
        }
        protected async Task EvaluateAnswer(int index)
        {
            if (mathQuestion.CorrectChoice != index)
            {
                answerCss = "wrong";
                answerMessage = "Nope, try again";
                await ShowHideMessage(false);
                return;
            }
            answerCss = "correct";
            answerMessage = "Nice. Here comes another";
            await ShowHideMessage(true);
        }

        protected async Task ShowHideMessage(bool isCorrect)
        {
            isDisplayMessage = !isDisplayMessage;
            StateHasChanged();
            await Task.Delay(3000);
            isDisplayMessage = !isDisplayMessage;
            StateHasChanged();
            if (isCorrect)
                await GetMathQuiz();
        }
        protected void Reset()
        {
            MathTopic = "";
            MathDifficulty = "";
            isSelectQuiz = !isSelectQuiz;
        }
    }
}

