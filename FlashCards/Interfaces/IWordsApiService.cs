using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Interfaces
{
    public interface IWordsApiService
    {
        Task<List<DefinitionModel>> GetDefinitions(bool isMany);
        Task<DefinitionModel> GetDefinitions(string word);
    }
}
