using System;
using System.Globalization;

namespace Maempedia.Models
{
    public class Menu
    {
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

        public Owner Owner { get; set; }

        public Menu()
        {
        }

        public Menu(LocalMenu localMenu)
        {
            this.ID = localMenu.ID;
            this.Name = localMenu.Name;
            this.Headline = localMenu.Headline;
            this.Portion = localMenu.Portion;
            this.ImageSource = localMenu.ImageSource;
            this.Price = localMenu.Price;
            this.Like = localMenu.Like;
            this.Promoted = localMenu.Promoted;
            this.PostID = localMenu.PostID;

            this.Owner = new Owner()
            {
                ID = localMenu.OwnerID,
                Username = localMenu.Username,
                Email = localMenu.Email,
                Name = localMenu.OwnerName,
                Headline = localMenu.Headline,
                OpeningHour = localMenu.OpeningHour,
                ClosingHour = localMenu.ClosingHour,
                ProfilePicture = localMenu.ProfilePicture,
                ContactNumber = localMenu.ContactNumber,
                ContactWA = localMenu.ContactWA,
                Location = new Location()
                {
                    Address = localMenu.Address,
                    City = localMenu.City,
                    Country = localMenu.Country,
                    Latitude = localMenu.Latitude,
                    Longitude = localMenu.Longitude
                }
            };
        }
    }
}
