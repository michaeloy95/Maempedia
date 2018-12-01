using System;

namespace Maempedia.Models
{
    public class NotificationItem
    {
        public string ID { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
        
        public string ImageSource { get; set; }

        public DateTime Time { get; set; }

        public string TimeString
        {
            get
            {
                return Time.ToString();
            }
        }
    }
}
