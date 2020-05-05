using FlashCards.Interfaces;
using FlashCards.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
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
            return client.Execute(request);
        }
        public async Task<WordQuizData> GetWordQuiz(string area, int level)
        {
            if (level > 10 || level < 1)
                return null;
            if (area != "sat" && area != "gre" && area != "hs" && area != "ms" && area != "es" && area != "gmat" && area != "overall")
                return null;
            level = SetQuizLevel(area, level);
            var response = GetResponse(area, level);
            var quizData = JsonConvert.DeserializeObject<WordQuizData>(response.Content);
            return await Task.FromResult(quizData);
        }
        private int SetQuizLevel(string area, int level)
        {
            if (area == "es" && level > 5)
                return 5;
            if (area == "ms" && level > 7)
                return 7;
            if (area == "hs" && level > 9)
                return 9;
            return level;
        }
    }
}
