using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Services
{
    public class FlashCardService
    {
        public List<Card> GetAltAnsers(List<Card> cards)
        {
            var answers = cards.Select(x => x.Answer).ToArray();
            var random = new Random();
            foreach (var card in cards)
            {
                var altAnswer = random.Next(0, answers.Length);
                (card.AltAnswers ?? (card.AltAnswers = new List<string>())).Add(answers[altAnswer]);
                altAnswer = random.Next(0, answers.Length);
                card.AltAnswers.Add(answers[altAnswer]);
            }
            return cards;
        }
    }
}
