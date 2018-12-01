using Newtonsoft.Json.Linq;

namespace Maempedia.Models
{
    public class Location
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Place_ID { get; set; }

        public double Distance { get; set; }

        public Location()
        {

        }

        public Location(JObject jsonObject)
        {
            Address = (string)jsonObject["result"]["name"];
            Latitude = (double)jsonObject["result"]["geometry"]["location"]["lat"];
            Longitude = (double)jsonObject["result"]["geometry"]["location"]["lng"];
        }
    }
}
