using Newtonsoft.Json;

namespace Maempedia.Models
{
    public class AutoCompleteLocation
    {
		[JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("id")]
        public string ID { get; set; }
        
        [JsonProperty("place_id")]
        public string Place_ID { get; set; }
        
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
