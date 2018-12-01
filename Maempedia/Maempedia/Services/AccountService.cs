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

namespace Maempedia.Services
{
    public static class AccountService
    {
        private static string apiKey = Constant.MAEMPEDIA_WEB_API_KEY;

        private const int TIMEOUT = 5;

        public static async Task<ServerResponseStatus> CheckUsernameIsValid(string username)
        {
            try
            {
                var url = "https://maempedia.com/check_username.html";
                var requestURI = $"{url}?username={username}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

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

                if (json["status"].ToString() == "VALID")
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

        public static async Task<ServerResponseStatus> CheckEmailIsValid(string email)
        {
            try
            {
                var url = "https://maempedia.com/check_email.html";
                var requestURI = $"{url}?email={email}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

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

                if (json["status"].ToString() == "VALID")
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

        public static async Task<ServerResponseStatus> CheckNameIsValid(string name)
        {
            try
            {
                var url = "https://maempedia.com/check_name.html";
                var requestURI = $"{url}?name={name}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

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

                if (json["status"].ToString() == "VALID")
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

        public static async Task<ServerResponseStatus> CheckContactIsValid(string contact)
        {
            try
            {
                var url = "https://maempedia.com/check_contact.html";
                var requestURI = $"{url}?contact={contact}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

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

                if (json["status"].ToString() == "VALID")
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

        public static async Task<ServerResponseStatus> TryLogin(string username, string password)
        {
            try
            { 
                var url = "https://maempedia.com/login.html";
                var requestURI = $"{url}?key={apiKey}";

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
                    Owner owner = new Owner()
                    {
                        ID = json["user"]["id"].ToString(),
                        Username = json["user"]["username"].ToString(),
                        Password = json["user"]["password"].ToString(),
                        Email = json["user"]["email"].ToString(),
                        Name = json["user"]["name"].ToString(),
                        ProfilePicture = "https://www." + json["user"]["photo_url"].ToString(),
                        ProfilePictureThumb = "https://www." + json["user"]["photo_thumb_url"].ToString(),
                        ContactNumber = json["user"]["contact"].ToString(),
                        ContactWA = json["user"]["wacontact"].ToString(),
                        IsMaemseller = bool.Parse(json["user"]["is_maemseller"].ToString())
                    };

                    if (owner.IsMaemseller)
                    {
                        owner.Headline = json["user"]["description"].ToString();
                        owner.OpeningHour = json["user"]["opening_hour"].ToString();
                        owner.ClosingHour = json["user"]["closing_hour"].ToString();
                        owner.Location = new Location()
                        {
                            Address = json["user"]["location"]["address"].ToString(),
                            Latitude = double.Parse(json["user"]["location"]["lat"].ToString()),
                            Longitude = double.Parse(json["user"]["location"]["lng"].ToString())
                        };
                    }

                    App.User.SetUser(owner);
                    App.User.ProfileSynchronised = true;

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

        public static async Task<ServerResponseStatus> TryRegister(string username, string password, string email, string contact)
        {
            try
            {
                var url = "https://maempedia.com/register.html";
                var requestURI = $"{url}?key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password },
                    { "email", email },
                    { "wacontact", contact },
                    { "name", username }
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
                    Owner newOwner = new Owner()
                    {
                        ID = json["owner"]["id"].ToString(),
                        Username = json["owner"]["username"].ToString(),
                        Password = json["owner"]["password"].ToString(),
                        Email = json["owner"]["email"].ToString(),
                        ContactWA = json["owner"]["wacontact"].ToString(),
                        Name = json["owner"]["username"].ToString(),
                        ProfilePicture = json["owner"]["photo_url"].ToString(),
                        ProfilePictureThumb = json["owner"]["photo_thumb_url"].ToString()
                    };

                    App.User.SetUser(newOwner);
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

        public static async Task<ServerResponseStatus> UpdateAccount(Models.Owner owner, byte[] image = null)
        {
            try
            {
                var url = "https://maempedia.com/update_account.html";
                var requestURI = $"{url}?id={owner.ID}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "username", owner.Username },
                    { "password", owner.Password },
                    { "email", owner.Email },
                    { "name", owner.Name },
                    { "opening_hour", owner.OpeningHour },
                    { "closing_hour", owner.ClosingHour },
                    { "description", owner.Headline },
                    { "location_address", owner.Location.Address },
                    { "location_lat", owner.Location.Latitude.ToString() },
                    { "location_lng", owner.Location.Longitude.ToString() },
                    { "wacontact", owner.ContactWA },
                    { "contact", owner.ContactNumber },
                    { "is_maemseller", owner.IsMaemseller.ToString() }
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

                return await UploadImage(image, owner.ID);
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
                Uri webService = new Uri("https://www.maempedia.com/owner_image.html");

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

        public static async Task<ServerResponseStatus> SendFeedback(string id, string comment, string suggestion)
        {
            try
            {
                var url = "https://maempedia.com/feedbacks.html";
                var requestURI = $"{url}?id={id}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "comment", comment },
                    { "suggestion", suggestion }
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

        public static async Task<ServerResponseStatus> ReportBugs(string id, string comment)
        {
            try
            {
                var url = "https://maempedia.com/bugs_report.html";
                var requestURI = $"{url}?id={id}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "comment", comment }
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
    }
}
