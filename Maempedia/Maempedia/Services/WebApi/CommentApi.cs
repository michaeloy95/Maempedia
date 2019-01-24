using Maempedia.Enum;
using Maempedia.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maempedia.Services.WebApi
{
    public class CommentApi : ApiRequest
    {
        public async Task<Tuple<IList<Comment>, int>> GetComments(string menuId, int start, int count = 0, string userId = null)
        {
            var uri = $"comments.html?menu_id={menuId}&start={start}&count={count}";
            if (userId != null)
            {
                uri += $"&user_id={userId}";
            }

            var data = await this.GetFromMaempedia(uri);
            if (data == null)
            {
                return null;
            }

            int total = int.Parse(data["total"].ToString());

            IList<Comment> commentList = new List<Comment>();

            if (total > 0)
            {
                foreach (JObject comment in data["comments"])
                {
                    commentList.Add(new Comment(comment));
                }
            }

            return Tuple.Create(commentList, total);
        }

        public async Task<ServerResponseStatus> AddComment(string message, string userId, string menuId, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "description", message },
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"add_comment.html?user_id={userId}&menu_id={menuId}", values);
            
            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> EditComment(string message, string commentId, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "description", message },
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"edit_comment.html?id={commentId}", values);

            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> DeleteComment(string commentId, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"delete_comment.html?id={commentId}", values);

            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }
    }
}
