using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class MathQuestionModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("choices")]
        public List<string> Choices { get; set; }

        [JsonProperty("correct_choice")]
        public int CorrectChoice { get; set; }

        [JsonProperty("instruction")]
        public string Instruction { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }
    }
}
