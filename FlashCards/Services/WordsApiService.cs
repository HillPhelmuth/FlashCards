using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Serializers.Newtonsoft;
using Newtonsoft.Json;
using RestSharp.Serialization;
using Microsoft.Extensions.Configuration;

namespace FlashCards.Services
{
    public class WordsApiService
    {
        private string _apiKey;
        private IConfiguration _configuration;

        public WordsApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["WordsApiKey"];
        }

        [HttpGet]
        public async Task<DefinitionModel> GetDefinitions(string word)
        {
            DefinitionModel result = new DefinitionModel();
            var client = new RestClient($"https://wordsapiv1.p.rapidapi.com/words/{word}/definitions");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", _apiKey);
            IRestResponse response = client.Execute(request);
            result = JsonConvert.DeserializeObject<DefinitionModel>(response.Content);
            return await Task.FromResult(result);
        }
    }
    //public class JsonNetSerializer : IRestSerializer
    //{
    //    public string Serialize(object obj) =>
    //        JsonConvert.SerializeObject(obj);

    //    public string Serialize(Parameter parameter) =>
    //        JsonConvert.SerializeObject(parameter.Value);

    //    public T Deserialize<T>(IRestResponse response) =>
    //        JsonConvert.DeserializeObject<T>(response.Content);

    //    public string[] SupportedContentTypes { get; } =
    //    {
    //        "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
    //    };

    //    public string ContentType { get; set; } = "application/json";

    //    public DataFormat DataFormat { get; } = DataFormat.Json;
    //}
}
