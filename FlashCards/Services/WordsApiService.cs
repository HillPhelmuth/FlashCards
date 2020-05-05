using System.Collections.Generic;
using System.Threading.Tasks;
using FlashCards.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using FlashCards.Interfaces;

namespace FlashCards.Services
{
    public class WordsApiService : IWordsApiService
    {
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;
        private const string randomString = "?random=true";
        public WordsApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["WordsApiKey"];
        }

        [HttpGet]
        public async Task<DefinitionModel> GetDefinitions(string word)
        {
            string requestString = $"{word}/definitions";
            DefinitionModel result = RequestWordsApi(requestString);
            return await Task.FromResult(result);
        }

        [HttpGet]
        public async Task<List<DefinitionModel>> GetDefinitions(bool isMany)
        {
            int loops = isMany ? 10 : 1;
            var resultList = new List<DefinitionModel>();
            const string requestString = randomString;
            for (int i = 0; i < loops; i++)
            {
                var result = RequestWordsApi(requestString);
                resultList.Add(result);
            }
            return await Task.FromResult(resultList);
        }
        private DefinitionModel RequestWordsApi(string requestString)
        {
            var client = new RestClient($"https://wordsapiv1.p.rapidapi.com/words/{requestString}");
            RestRequest request = CreateWordsApiRestRequest();
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<DefinitionModel>(response.Content);
        }

        private RestRequest CreateWordsApiRestRequest()
        {
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", _apiKey);
            return request;
        }
    }
}
