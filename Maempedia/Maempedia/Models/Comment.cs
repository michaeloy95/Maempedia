using System;
using Newtonsoft.Json.Linq;

namespace Maempedia.Models
{
    public class Comment
    {
        public string ID { get; set; }

        public string UserID { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string PhotoThumbURL { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public string DateTimeText
        {
            get
            {
                TimeSpan ts = DateTime.Now.Subtract(this.DateTime);
                return (ts.TotalMinutes < 1) ? $"{ts.Seconds}s"
                    : (ts.TotalHours < 1) ? $"{ts.Minutes}m"
                    : (ts.TotalDays < 1) ? $"{ts.Hours}h"
                    : (ts.TotalDays < 365) ? $"{ts.Days}d"
                    : $"{(int)(ts.TotalDays / 365)}y";
            }
        }

        public Comment()
        {
        }

        public Comment(JObject jsonObject)
        {
            this.ID = jsonObject["id"].ToString();
            this.UserID = jsonObject["user_id"].ToString();
            this.Username = jsonObject["username"].ToString();
            this.Name = jsonObject["name"].ToString();
            this.PhotoThumbURL = $"https://maempedia.com/{jsonObject["photo_thumb_url"].ToString()}";
            this.Message = jsonObject["message"].ToString();
            this.DateTime = DateTime.ParseExact(
                jsonObject["date"].ToString(),
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
