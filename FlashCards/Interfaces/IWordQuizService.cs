using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Interfaces
{
    public interface IWordQuizService
    {
        Task<WordQuizData> GetWordQuiz(string area, int level);
    }
}
