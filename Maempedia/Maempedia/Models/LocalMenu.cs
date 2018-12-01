using SQLite;
using System;
using System.Globalization;

namespace Maempedia.Models
{
    public class LocalMenu
    {
        [PrimaryKey]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Headline { get; set; }

        public string Portion { get; set; }

        public string ImageSource { get; set; }

        public float Price { get; set; }

        public string PriceString
        {
            get
            {
                return String.Format(new CultureInfo("id-ID"), "Rp. {0:N}", this.Price);
            }
        }

        public int Like { get; set; }

        public bool Promoted { get; set; }

        public string PostID { get; set; }

        public string OwnerID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string OwnerName { get; set; }

        public string OwnerHeadline { get; set; }

        public string OpeningHour { get; set; }

        public string ClosingHour { get; set; }

        public string ProfilePicture { get; set; }

        public string ContactNumber { get; set; }

        public string ContactWA { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public LocalMenu()
        {
        }

        public LocalMenu(Models.Menu menu)
        {
            this.ID = menu.ID;
            this.Name = menu.Name;
            this.Headline = menu.Headline;
            this.Portion = menu.Portion;
            this.ImageSource = menu.ImageSource;
            this.Price = menu.Price;
            this.Like = menu.Like;
            this.Promoted = menu.Promoted;
            this.PostID = menu.PostID;
            this.OwnerID = menu.Owner.ID;
            this.Username = menu.Owner.Email;
            this.Email = menu.Owner.Email;
            this.OwnerName = menu.Owner.Name;
            this.OwnerHeadline = menu.Owner.Headline;
            this.OpeningHour = menu.Owner.OpeningHour;
            this.ClosingHour = menu.Owner.ClosingHour;
            this.ProfilePicture = menu.Owner.ProfilePicture;
            this.ContactNumber = menu.Owner.ContactNumber;
            this.ContactWA = menu.Owner.ContactWA;
            this.Address = menu.Owner.Location.Address;
            this.City = menu.Owner.Location.City;
            this.Country = menu.Owner.Location.Country;
            this.Latitude = menu.Owner.Location.Latitude;
            this.Longitude = menu.Owner.Location.Longitude;
        }
    }
}
