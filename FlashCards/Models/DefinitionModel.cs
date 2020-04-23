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
    }

    public partial class DefinitionData
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("partOfSpeech")]
        public string PartOfSpeech { get; set; }
    }
}
