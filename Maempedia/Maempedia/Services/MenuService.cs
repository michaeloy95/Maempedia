using Maempedia.Common;
using Maempedia.Enum;
using Maempedia.Models;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static Maempedia.Views.Browse.BrowsePage;

namespace Maempedia.Services
{
    public static class MenuService
    {
        private static string apiKey = Constant.MAEMPEDIA_WEB_API_KEY;

        private const int TIMEOUT = 5;

        public static async Task<IList<Menu>> GetMenus(int start, SortMenuBy sortBy, string keyword)
        {
            try
            {
                const int count = 5;

                string type = sortBy == SortMenuBy.Nearby ? "Terdekat"
                : sortBy == SortMenuBy.Trending ? "Trending"
                : sortBy == SortMenuBy.Latest ? "Terbaru"
                : string.Empty;

                var url = "https://maempedia.com/menus.html";
                var requestURI = $"{url}?type={type}&start={start}&count={count}&keyword={keyword}&key={apiKey}";

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

                var menu_list = new List<Menu>();
                foreach (JObject menu in json["menu"])
                {
                    menu_list.Add(new Menu()
                    {
                        ID = menu["id"].ToString(),
                        Name = menu["name"].ToString(),
                        Headline = menu["description"].ToString(),
                        Portion = menu["portion"].ToString(),
                        ImageSource = "https://www." + menu["photo_url"].ToString(),
                        Price = float.Parse(menu["price"].ToString()),
                        Like = int.Parse(menu["like"].ToString()),
                        Promoted = bool.Parse(menu["promoted"].ToString()),
                        PostID = menu["post_id"].ToString(),
                        Owner = new Owner()
                        {
                            ID = menu["owner"]["id"].ToString(),
                            Name = menu["owner"]["name"].ToString(),
                            Username = menu["owner"]["username"].ToString(),
                            Email = menu["owner"]["email"].ToString(),
                            Headline = menu["owner"]["description"].ToString(),
                            OpeningHour = menu["owner"]["opening_hour"].ToString(),
                            ClosingHour = menu["owner"]["closing_hour"].ToString(),
                            ProfilePicture = "https://www." + menu["owner"]["photo_url"].ToString(),
                            ProfilePictureThumb = "https://www." + menu["owner"]["photo_thumb_url"].ToString(),
                            ContactNumber = menu["owner"]["contact"].ToString(),
                            ContactWA = menu["owner"]["wacontact"].ToString(),
                            Location = new Location()
                            {
                                Address = menu["owner"]["location"]["address"].ToString(),
                                Latitude = double.Parse(menu["owner"]["location"]["lat"].ToString()),
                                Longitude = double.Parse(menu["owner"]["location"]["lng"].ToString()),
                                Distance = menu["owner"]["location"]["distance"].ToString() == "N/A" ? 0
                                            : double.Parse(menu["owner"]["location"]["distance"].ToString())
                            }
                        }
                    });
                }

                return menu_list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<IList<Menu>> GetMenus(double lat, double lng, int start, SortMenuBy sortBy, string keyword)
        {
            try
            {
                const int count = 5;

                string type = sortBy == SortMenuBy.Nearby ? "Terdekat"
                : sortBy == SortMenuBy.Trending ? "Trending"
                : sortBy == SortMenuBy.Latest ? "Terbaru"
                : string.Empty;

                var url = "https://maempedia.com/menus.html";
                var requestURI = $"{url}?lat={lat}&lng={lng}&type={type}&start={start}&count={count}&keyword={keyword}&key={apiKey}";
                
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

                var menu_list = new List<Menu>();
                foreach (JObject menu in json["menu"])
                {
                    menu_list.Add(new Menu()
                    {
                        ID = menu["id"].ToString(),
                        Name = menu["name"].ToString(),
                        Headline = menu["description"].ToString(),
                        Portion = menu["portion"].ToString(),
                        ImageSource = "https://www." + menu["photo_url"].ToString(),
                        Price = float.Parse(menu["price"].ToString()),
                        Like = int.Parse(menu["like"].ToString()),
                        Promoted = bool.Parse(menu["promoted"].ToString()),
                        PostID = menu["post_id"].ToString(),
                        Owner = new Owner()
                        {
                            ID = menu["owner"]["id"].ToString(),
                            Name = menu["owner"]["name"].ToString(),
                            Username = menu["owner"]["username"].ToString(),
                            Email = menu["owner"]["email"].ToString(),
                            Headline = menu["owner"]["description"].ToString(),
                            OpeningHour = menu["owner"]["opening_hour"].ToString(),
                            ClosingHour = menu["owner"]["closing_hour"].ToString(),
                            ProfilePicture = "https://www." + menu["owner"]["photo_url"].ToString(),
                            ProfilePictureThumb = "https://www." + menu["owner"]["photo_thumb_url"].ToString(),
                            ContactNumber = menu["owner"]["contact"].ToString(),
                            ContactWA = menu["owner"]["wacontact"].ToString(),
                            Location = new Location()
                            {
                                Address = menu["owner"]["location"]["address"].ToString(),
                                Latitude = double.Parse(menu["owner"]["location"]["lat"].ToString()),
                                Longitude = double.Parse(menu["owner"]["location"]["lng"].ToString()),
                                Distance = menu["owner"]["location"]["distance"].ToString() == "N/A" ? 0
                                            : double.Parse(menu["owner"]["location"]["distance"].ToString())
                            }
                        }
                    });
                }

                return menu_list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<IList<Menu>> GetMenusFromOwnerId(Models.Owner owner)
        {
            try
            {
                var url = "https://maempedia.com/menus.html";
                var requestURI = $"{url}?user_id={owner.ID}&key={apiKey}";

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

                var menu_list = new List<Menu>();
                foreach (JObject menu in json["menu"])
                {
                    menu_list.Add(new Menu()
                    {
                        ID = menu["id"].ToString(),
                        Name = menu["name"].ToString(),
                        Headline = menu["description"].ToString(),
                        Portion = menu["portion"].ToString(),
                        ImageSource = "https://www." + menu["photo_url"].ToString(),
                        Price = float.Parse(menu["price"].ToString()),
                        Like = int.Parse(menu["like"].ToString()),
                        Promoted = bool.Parse(menu["promoted"].ToString()),
                        PostID = menu["post_id"].ToString(),
                        Owner = owner
                    });
                }

                return menu_list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<Menu> GetMenu(string menuId)
        {
            try
            {
                var url = "https://maempedia.com/menus.html";
                var requestURI = $"{url}?menu_id={menuId}&key={apiKey}";

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

                var menu = new Menu()
                {
                    ID = json["id"].ToString(),
                    Name = json["name"].ToString(),
                    Headline = json["description"].ToString(),
                    Portion = json["portion"].ToString(),
                    ImageSource = "https://www." + json["photo_url"].ToString(),
                    Price = float.Parse(json["price"].ToString()),
                    Like = int.Parse(json["like"].ToString()),
                    Promoted = bool.Parse(json["promoted"].ToString()),
                    PostID = json["post_id"].ToString(),
                    Owner = new Owner()
                    {
                        ID = json["owner"]["id"].ToString(),
                        Name = json["owner"]["name"].ToString(),
                        Username = json["owner"]["username"].ToString(),
                        Email = json["owner"]["email"].ToString(),
                        Headline = json["owner"]["description"].ToString(),
                        OpeningHour = json["owner"]["opening_hour"].ToString(),
                        ClosingHour = json["owner"]["closing_hour"].ToString(),
                        ProfilePicture = "https://www." + json["owner"]["photo_url"].ToString(),
                        ProfilePictureThumb = "https://www." + json["owner"]["photo_thumb_url"].ToString(),
                        ContactNumber = json["owner"]["contact"].ToString(),
                        ContactWA = json["owner"]["wacontact"].ToString(),
                        Location = new Location()
                        {
                            Address = json["owner"]["location"]["address"].ToString(),
                            Latitude = double.Parse(json["owner"]["location"]["lat"].ToString()),
                            Longitude = double.Parse(json["owner"]["location"]["lng"].ToString()),
                            Distance = json["owner"]["location"]["distance"].ToString() == "N/A" ? 0
                                            : double.Parse(json["owner"]["location"]["distance"].ToString())
                        }
                    }
                };

                return menu;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<ServerResponseStatus> AddMenu(Models.Menu menu, byte[] image, string ownerId, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/add_menu.html";
                var requestURI = $"{url}?owner_id={ownerId}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "name", menu.Name },
                    { "description", menu.Headline },
                    { "portion", menu.Portion },
                    { "price", menu.Price.ToString() },
                    { "username", username },
                    { "password", password }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(requestURI, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return ServerResponseStatus.ERROR;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return ServerResponseStatus.ERROR;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return ServerResponseStatus.ERROR;
                }

                if (json["status"].ToString() != "SUCCESS")
                {
                    return ServerResponseStatus.INVALID;
                }

                string menuId = json["menu"]["id"].ToString();
                return await UploadImage(image, menuId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<ServerResponseStatus> UpdateMenu(Models.Menu menu, byte[] image, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/update_menu.html";
                var requestURI = $"{url}?id={menu.ID}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "name", menu.Name },
                    { "description", menu.Headline },
                    { "portion", menu.Portion },
                    { "price", menu.Price.ToString() },
                    { "like", menu.Like.ToString() },
                    { "promoted ", menu.Promoted.ToString() },
                    { "username", username },
                    { "password", password }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(requestURI, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return ServerResponseStatus.ERROR;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return ServerResponseStatus.ERROR;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return ServerResponseStatus.ERROR;
                }

                if (json["status"].ToString() != "SUCCESS")
                {
                    return ServerResponseStatus.INVALID;
                }

                if (image == null)
                {
                    return ServerResponseStatus.VALID;
                }

                return await UploadImage(image, menu.ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<ServerResponseStatus> DeleteMenu(string id, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/delete_menu.html";
                var requestURI = $"{url}?id={id}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(requestURI, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return ServerResponseStatus.ERROR;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return ServerResponseStatus.ERROR;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return ServerResponseStatus.ERROR;
                }

                if (json["status"].ToString() == "SUCCESS")
                {
                    return ServerResponseStatus.VALID;
                }
                else
                {
                    return ServerResponseStatus.INVALID;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<ServerResponseStatus> UploadImage(byte[] image, string id)
        {
            try
            {
                Uri webService = new Uri("https://www.maempedia.com/menu_image.html");

                var byteArrayContent = new ByteArrayContent(image);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");

                var content = new MultipartFormDataContent();
                content.Add(byteArrayContent, "image_main", $"{id}b.jpg");

                content.Add(new StringContent(id), "id");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
                requestMessage.Headers.ExpectContinue = false;
                requestMessage.Content = content;

                HttpClient client = new HttpClient();

                var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return ServerResponseStatus.ERROR;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return ServerResponseStatus.ERROR;
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return ServerResponseStatus.ERROR;
                }

                if (json["status"].ToString() == "SUCCESS")
                {
                    return ServerResponseStatus.VALID;
                }
                else
                {
                    return ServerResponseStatus.INVALID;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<Tuple<ServerResponseStatus, string>> RequestPromotion(string id, int price, int days_count, string voucher_code, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/set_ads.html";
                var requestURI = $"{url}?id={id}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {   
                    { "price", price.ToString() },
                    { "days_count", days_count.ToString() },
                    { "voucher_code", voucher_code },
                    { "username", username },
                    { "password", password }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(requestURI, content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Maempedia HTTP request denied.");
                    return Tuple.Create(ServerResponseStatus.ERROR, string.Empty);
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == null)
                {
                    Debug.WriteLine("Maempedia API has returned nothing.");
                    return Tuple.Create(ServerResponseStatus.ERROR, string.Empty);
                }

                JObject json = null;
                try
                {
                    json = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                    return Tuple.Create(ServerResponseStatus.ERROR, string.Empty);
                }

                if (json["status"].ToString() == "SUCCESS")
                {
                    return Tuple.Create(ServerResponseStatus.VALID, json["referencecode"].ToString());
                }
                else
                {
                    return Tuple.Create(ServerResponseStatus.INVALID, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Tuple.Create(ServerResponseStatus.ERROR, string.Empty);
            }
        }
    }
}
