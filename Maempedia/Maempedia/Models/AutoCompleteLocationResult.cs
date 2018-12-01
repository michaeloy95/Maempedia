using Newtonsoft.Json;
using System.Collections.Generic;

namespace Maempedia.Models
{
    public class AutoCompleteLocationResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("predictions")]
        public List<AutoCompleteLocation> AutoCompletePlaces { get; set; }
    }
}
