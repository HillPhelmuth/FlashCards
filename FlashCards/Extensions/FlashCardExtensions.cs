using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Extensions
{
    public static class FlashCardExtensions
    {
        private static readonly Random rng = new Random();
        public static List<Card> AddAltAnswers(this List<Card> cards)
        {

            var answers = cards.Select(x => x.Answer).ToArray();
            foreach (var card in cards)
            {
                int altAnswer;
                if (card.DisplayAnswers == null)
                    card.DisplayAnswers = new List<AnswerData>();


                for (int i = 0; i < 3; i++)
                {
                    var altDisplayLoop = new AnswerData();
                    altAnswer = rng.Next(0, answers.Length);
                    altDisplayLoop.Answer = answers[altAnswer];
                    card.DisplayAnswers.Add(altDisplayLoop);
                }
                var altDisplay = new AnswerData() { Answer = card.Answer };
                altDisplay.Answer = card.Answer;
                card.DisplayAnswers.Add(altDisplay);
            }
            return cards;
        }
        public static void Shuffle<T>(this IList<T> cards)
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
        public static bool IsCorrectAnswer(this string selectedAnswer, string cardAnswer)
        {
            return selectedAnswer == cardAnswer;
        }
    }
}
