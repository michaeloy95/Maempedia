using Maempedia.Common;
using Maempedia.Models;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maempedia.Services
{
    public class OwnerService
    {
        private static string apiKey = Constant.MAEMPEDIA_WEB_API_KEY;

        public static async Task<IList<Owner>> GetNearbyOwners(double lat, double lng, int max)
        {
            try
            {
                var url = "https://maempedia.com/owners.html";
                var requestURI = $"{url}?lat={lat}&lng={lng}&max={max}&key={apiKey}";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return null;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return null;
                }

                var owner_list = new List<Owner>();
                foreach(JObject owner in json["owner"])
                {
                    owner_list.Add(new Owner()
                    {
                        ID = owner["id"].ToString(),
                        Username = owner["username"].ToString(),
                        Email = owner["email"].ToString(),
                        Name = owner["name"].ToString(),
                        Headline = owner["description"].ToString(),
                        OpeningHour = owner["opening_hour"].ToString(),
                        ClosingHour = owner["closing_hour"].ToString(),
                        ProfilePicture = "https://www." + owner["photo_url"].ToString(),
                        ProfilePictureThumb = "https://www." + owner["photo_thumb_url"].ToString(),
                        ContactNumber = owner["contact"].ToString(),
                        ContactWA = owner["wacontact"].ToString(),
                        Location = new Location()
                        {
                            Address = owner["location"]["address"].ToString(),
                            Latitude = double.Parse(owner["location"]["lat"].ToString()),
                            Longitude = double.Parse(owner["location"]["lng"].ToString()),
                            Distance = double.Parse(owner["location"]["distance"].ToString())
                        }
                    });
                }

                return owner_list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<Owner> GetOwner(string ownerId)
        {
            try
            {
                var url = "https://maempedia.com/owner_details.html";
                var requestURI = $"{url}?id={ownerId}&key={apiKey}";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return null;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return null;
                }

                // Handle empty latitude and longitude
                double latTmp = json["location"]?["lat"]?.ToString() == "" ? 0
                    : double.Parse(json["location"]?["lat"]?.ToString());
                double lngTmp = json["location"]?["lng"]?.ToString() == "" ? 0
                    : double.Parse(json["location"]?["lng"]?.ToString());

                var owner = new Owner()
                {
                    ID = json["id"]?.ToString(),
                    Username = json["username"]?.ToString(),
                    Email = json["email"]?.ToString(),
                    Name = json["name"]?.ToString(),
                    Headline = json["description"]?.ToString(),
                    OpeningHour = json["opening_hour"]?.ToString(),
                    ClosingHour = json["closing_hour"]?.ToString(),
                    ProfilePicture = "https://www." + json["photo_url"]?.ToString(),
                    ProfilePictureThumb = "https://www." + json["photo_thumb_url"]?.ToString(),
                    ContactNumber = json["contact"]?.ToString(),
                    ContactWA = json["wacontact"]?.ToString(),
                    IsMaemseller = bool.Parse(json["is_maemseller"].ToString()),
                    Location = new Location()
                    {
                        Address = json["location"]?["address"]?.ToString(),
                        Latitude = latTmp,
                        Longitude = lngTmp
                    }
                };

                return owner;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }
    }
}
