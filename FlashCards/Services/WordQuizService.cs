using FlashCards.Interfaces;
using FlashCards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Services
{
    
    public class WordQuizService : IWordQuizService
    {
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl = "twinword-word-association-quiz.p.rapidapi.com";


        public WordQuizService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["WordsApiKey"];
        }
        private RestRequest CreateWordsApiRestRequest()
        {
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", _baseUrl);
            request.AddHeader("x-rapidapi-key", _apiKey);
            return request;
        }
        private IRestResponse GetResponse(string area, int level)
        {
            var client = new RestClient($"https://{_baseUrl}/type1/?area={area}&level={level}");
            RestRequest request = CreateWordsApiRestRequest();
            IRestResponse response = client.Execute(request);
            return response;

        }
        public async Task<WordQuizData> GetWordQuiz(string area, int level)
        {
            if (level > 10 || level < 1)
                return null;
            if (area != "sat" && area != "gre" && area != "hs" && area != "ms" && area != "es" && area != "gmat" && area != "overall")
                return null;
            var response = GetResponse(area, level);
            var quizData = JsonConvert.DeserializeObject<WordQuizData>(response.Content);
            return await Task.FromResult(quizData);
        }
    }
}
