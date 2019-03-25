using Newtonsoft.Json.Linq;
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

        public double Discount { get; set; }

        public int DiscountDaysLeft { get; set; }

        public int MaxClaim { get; set; }

        public int RemainingClaim { get; set; }

        public Menu()
        {
            this.ID = string.Empty;
            this.Like = 0;
            this.Portion = string.Empty;
            this.ImageSource = string.Empty;
            this.Price = 0;
            this.Promoted = false;
            this.PostID = string.Empty;
            this.Owner = new Owner();
            this.Discount = 0;
            this.DiscountDaysLeft = 0;
            this.MaxClaim = 0;
            this.RemainingClaim = 0;
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

        public Menu (JObject json, Owner owner = null)
        {
            this.ID = json["id"].ToString();
            this.Name = json["name"].ToString();
            this.Headline = json["description"].ToString();
            this.Portion = json["portion"].ToString();
            this.ImageSource = "https://www." + json["photo_url"].ToString();
            this.Price = float.Parse(json["price"].ToString());
            this.Like = int.Parse(json["like"].ToString());
            this.Promoted = bool.Parse(json["promoted"].ToString());
            this.PostID = json["post_id"].ToString();
            this.Discount = double.Parse(json["discount_percentage"].ToString());
            this.DiscountDaysLeft = int.Parse(json["discount_days_left"].ToString());
            this.MaxClaim = int.Parse(json["maximal_claim"].ToString());
            this.RemainingClaim = int.Parse(json["claim_left"].ToString());
            this.Owner = owner ?? new Owner((JObject)json["owner"]);
        }
    }
}
