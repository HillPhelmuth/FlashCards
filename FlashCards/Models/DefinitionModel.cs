using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    [Serializable]
    public class DefinitionModel
    {
        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("definitions")]
        public List<DefinitionData> Definitions { get; set; }
        [JsonProperty("results")]
        public List<DefinitionData> RandomDefinitions { get; set; }
    }

    public class DefinitionData
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("partOfSpeech")]
        public string PartOfSpeech { get; set; }
        [JsonProperty("synonyms")]
        public List<string> Synonyms { get; set; }
        [JsonProperty("typeOf")]
        public List<string> TypeOf { get; set; }
    }
}
