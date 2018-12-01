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
