using Newtonsoft.Json.Linq;
using System;

namespace Maempedia.Models
{
    public class Owner
    {
        public Owner()
        {
            this.IsMaemseller = false;
            this.Name = this.Headline = this.ContactNumber = this.ContactWA = "";
            this.OpeningHour = this.ClosingHour = "0";
        }

        public Owner(JObject json)
        {
            this.ID = json["id"].ToString();
            this.Username = json["username"].ToString();
            this.Password = json["password"].ToString();
            this.Email = json["email"].ToString();
            this.Name = json["name"].ToString();
            this.ProfilePicture = "https://www." + json["photo_url"].ToString();
            this.ProfilePictureThumb = "https://www." + json["photo_thumb_url"].ToString();
            this.ContactNumber = json["contact"].ToString();
            this.ContactWA = json["wacontact"].ToString();
            this.IsMaemseller = bool.Parse(json["is_maemseller"].ToString());

            if (this.IsMaemseller)
            {
                this.Headline = json["description"].ToString();
                this.OpeningHour = json["opening_hour"].ToString();
                this.ClosingHour = json["closing_hour"].ToString();

                this.Location = new Location();
                this.Location.Address = json["location"]["address"].ToString();
                this.Location.Latitude = json["location"]?["lat"]?.ToString() == "" ? 0
                    : double.Parse(json["location"]?["lat"]?.ToString());
                this.Location.Longitude = json["location"]?["lng"]?.ToString() == "" ? 0
                    : double.Parse(json["location"]?["lng"]?.ToString());
                this.Location.Distance = json["location"]["distance"].ToString() == "N/A" ? 0
                    : double.Parse(json["location"]["distance"].ToString());
            }
        }

        public string ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsMaemseller { get; set; }

        public string Name { get; set; }

        public string Headline { get; set; }

        public string OpeningHour { get; set; }

        public string ClosingHour { get; set; }

        public string ProfilePicture { get; set; }

        public string ProfilePictureThumb { get; set; }

        public string ContactNumber { get; set; }

        public string ContactWA { get; set; }

        public Location Location { get; set; }

        public string Distance
        {
            get
            {
                string distance = this.Location.Distance == 0 ? "-"
                    : this.Location.Distance < 0.01 ? "~10 M"
                    : this.Location.Distance < 1 ? $"{Math.Round(this.Location.Distance, 3) * 1000} M"
                    : $"{Math.Round(this.Location.Distance, 1)} KM";

                return distance;
            }
        }
    }
}
