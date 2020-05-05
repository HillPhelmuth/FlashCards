using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Interfaces
{
    public interface IMathQuizService
    {
        Task<MathQuestionModel> GetMathQuestion(string topic, string difficulty);
    }
}
