using Maempedia.Common;
using Maempedia.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maempedia.Services
{
    public static class AutoCompleteLocationService
    {
        private static string apiKey = Constant.GOOGLE_PLACES_WEB_API_KEY;

        public static async Task<Location> GetPlace(string placeID)
        {
            try
            {
                var requestURI = CreateDetailsRequestUri(placeID, apiKey);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Debug.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }

                return new Location(JObject.Parse(result));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<AutoCompleteLocationResult> GetPlaces(string keyword)
        {
            return await GetPlaces(keyword, "-7.25,112.75");
        }

        public static async Task<AutoCompleteLocationResult> GetPlaces(string keyword, string location)
        {
            try
            {
                var requestURI = CreatePredictionsUri(keyword, location, 20000, apiKey);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Debug.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }

                return JsonConvert.DeserializeObject<AutoCompleteLocationResult>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        static string CreateDetailsRequestUri(string place_id, string apiKey)
        {
            var url = "https://maps.googleapis.com/maps/api/place/details/json";
            return $"{url}?placeid={Uri.EscapeUriString(place_id)}&key={apiKey}";
        }

        static string CreatePredictionsUri(string keyword, string location, int radius, string apiKey)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
            var input = Uri.EscapeUriString(keyword);
            return $"{url}?input={input}&location={location}&radius={radius}&key={apiKey}";
        }
    }
}
