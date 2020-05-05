using System;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;

namespace FlashCards.Services
{
    public class MathQuizService : IMathQuizService
    {
        private readonly string _apiToken;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        public MathQuizService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _apiToken = _configuration["MathApiKey"];
            _clientFactory = httpClientFactory;
        }

        [HttpGet]
        private HttpResponseMessage GetMathQuizResponse(string topic = "simple", string difficulty = "", string area = "arithmetic")
        {
            if (topic == "equations-containing-absolute-values")
                area = "algebra";
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://studycounts.com/api/v1/{area}/{topic}.json?difficulty={difficulty}"),
                Headers =
                {
                    { nameof(HttpRequestHeader.Authorization), $"Bearer {_apiToken}" },
                    { nameof(HttpResponseHeader.ContentType), "application/json" }
                }
            };
            var response = client.SendAsync(request).Result;
            return response;
        }

        [HttpGet]
        public async Task<MathQuestionModel> GetMathQuestion(string topic, string difficulty)
        {
            var response = GetMathQuizResponse(topic, difficulty);
            var content = await response.Content.ReadAsStringAsync();
            var quizData = JsonConvert.DeserializeObject<MathQuestionModel>(content);
            return await Task.FromResult(quizData);
        }
    }
}
