using Maempedia.Common;
using Maempedia.Enum;
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
    public static class CommentService
    {
        private static string apiKey = Constant.MAEMPEDIA_WEB_API_KEY;

        private const int TIMEOUT = 5;

        public static async Task<Tuple<IList<Comment>, int>> GetComments(string menuId, int start, int count = 0, string userId = null)
        {
            try
            {
                var url = "https://maempedia.com/comments.html";
                var requestURI = $"{url}?menu_id={menuId}&start={start}&count={count}&key={apiKey}";
                requestURI += (!string.IsNullOrWhiteSpace(userId)) ? $"user_id={userId}" : "";

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

                int total = int.Parse(json["total"].ToString());

                IList<Comment> comment_list = new List<Comment>();

                if (total > 0)
                {
                    foreach (JObject comment in json["comments"])
                    {
                        comment_list.Add(new Comment(comment));
                    }
                }

                return Tuple.Create(comment_list, total);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Maempedia HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        public static async Task<ServerResponseStatus> AddComment(string message, string userId, string menuId, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/add_comment.html";
                var requestURI = $"{url}?user_id={userId}&menu_id={menuId}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "description", message },
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

                return ServerResponseStatus.VALID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<ServerResponseStatus> EditComment(string message, string commentId, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/edit_comment.html";
                var requestURI = $"{url}?id={commentId}&key={apiKey}";

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(TIMEOUT)
                };

                var values = new Dictionary<string, string>
                {
                    { "description", message },
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

                return ServerResponseStatus.VALID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }

        public static async Task<ServerResponseStatus> DeleteComment(string commentId, string username, string password)
        {
            try
            {
                var url = "https://maempedia.com/delete_comment.html";
                var requestURI = $"{url}?id={commentId}&key={apiKey}";

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

                if (json["status"].ToString() != "SUCCESS")
                {
                    return ServerResponseStatus.INVALID;
                }

                return ServerResponseStatus.VALID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return ServerResponseStatus.ERROR;
            }
        }
    }
}
