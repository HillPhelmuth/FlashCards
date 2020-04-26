using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class WordQuizData
    {
        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("quizlist")]
        public List<QuizData> Quizlist { get; set; }
    }

    public class QuizData
    {
        [JsonProperty("quiz")]
        public List<string> Quiz { get; set; }

        [JsonProperty("option")]
        public List<string> Option { get; set; }

        [JsonProperty("correct")]
        public int Correct { get; set; }        
        
    }
}
