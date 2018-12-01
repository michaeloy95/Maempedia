using Maempedia.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Maempedia.Data
{
    public class User
    {
        private const string LoggedInKey = "LoggedIn";

        private const string IDKey = "ID";

        private const string UsernameKey = "Username";

        private const string PasswordKey = "Password";

        private const string EmailKey = "Email";

        private const string NameKey = "Name";

        private const string HeadlineKey = "Headline";

        private const string OpeningHourKey = "OpeningHour";

        private const string ClosingHourKey = "ClosingHour";

        private const string ProfilePictureKey = "ProfilePicture";

        private const string ProfilePictureThumbKey = "ProfilePictureThumb";

        private const string ContactNumberKey = "ContactNumber";

        private const string ContactWAKey = "ContactWA";

        private const string IsMaemsellerKey = "IsMaemseller";

        private const string PlaceIDKey = "PlaceID";

        private const string AddressKey = "Address";

        private const string LatitudeKey = "Latitude";

        private const string LongitudeKey = "Longitude";

        public bool HasLoggedIn
        {
            get;
            private set;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public string Email
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Headline
        {
            get;
            private set;
        }

        public string OpeningHour
        {
            get;
            private set;
        }

        public string ClosingHour
        {
            get;
            private set;
        }

        public string ProfilePicture
        {
            get;
            private set;
        }

        public string ProfilePictureThumb
        {
            get;
            private set;
        }

        public string ContactNumber
        {
            get;
            private set;
        }

        public string ContactWA
        {
            get;
            private set;
        }

        public bool IsMaemseller
        {
            get;
            private set;
        }

        public string PlaceID
        {
            get;
            private set;
        }

        public string Address
        {
            get;
            private set;
        }

        public double Latitude
        {
            get;
            private set;
        }

        public double Longitude
        {
            get;
            private set;
        }

        public bool ProfileSynchronised
        {
            get;
            set;
        }

        public ObservableCollection<Models.Menu> MenuList
        {
            get;
            private set;
        }

        public bool MenuListFetched
        {
            get;
            set;
        }

        public User()
        {
            this.Initialise();
        }

        public void Initialise()
        {
            this.HasLoggedIn = Application.Current.Properties.ContainsKey(LoggedInKey)
                                    ? (bool)Application.Current.Properties[LoggedInKey]
                                    : false;

            if (!this.HasLoggedIn)
            {
                this.RemoveUser();
                return;
            }

            this.ID = Application.Current.Properties.ContainsKey(IDKey)
                                    ? (string)Application.Current.Properties[IDKey]
                                    : string.Empty;

            this.Username = Application.Current.Properties.ContainsKey(UsernameKey)
                                    ? (string)Application.Current.Properties[UsernameKey]
                                    : string.Empty;

            this.Password = Application.Current.Properties.ContainsKey(PasswordKey)
                                    ? (string)Application.Current.Properties[PasswordKey]
                                    : string.Empty;

            this.Email = Application.Current.Properties.ContainsKey(EmailKey)
                                    ? (string)Application.Current.Properties[EmailKey]
                                    : string.Empty;

            this.Name = Application.Current.Properties.ContainsKey(NameKey)
                                    ? (string)Application.Current.Properties[NameKey]
                                    : string.Empty;

            this.Headline = Application.Current.Properties.ContainsKey(HeadlineKey)
                                    ? (string)Application.Current.Properties[HeadlineKey]
                                    : string.Empty;

            this.OpeningHour = Application.Current.Properties.ContainsKey(OpeningHourKey)
                                    ? (string)Application.Current.Properties[OpeningHourKey]
                                    : "0";

            this.ClosingHour = Application.Current.Properties.ContainsKey(ClosingHourKey)
                                    ? (string)Application.Current.Properties[ClosingHourKey]
                                    : "0";

            this.ProfilePicture = Application.Current.Properties.ContainsKey(ProfilePictureKey)
                                    ? (string)Application.Current.Properties[ProfilePictureKey]
                                    : string.Empty;

            this.ProfilePictureThumb = Application.Current.Properties.ContainsKey(ProfilePictureThumbKey)
                                    ? (string)Application.Current.Properties[ProfilePictureThumbKey]
                                    : string.Empty;

            this.ContactNumber = Application.Current.Properties.ContainsKey(ContactNumberKey)
                                    ? (string)Application.Current.Properties[ContactNumberKey]
                                    : string.Empty;

            this.ContactWA = Application.Current.Properties.ContainsKey(ContactWAKey)
                                    ? (string)Application.Current.Properties[ContactWAKey]
                                    : string.Empty;

            this.IsMaemseller = Application.Current.Properties.ContainsKey(IsMaemsellerKey)
                                    ? (bool)Application.Current.Properties[IsMaemsellerKey]
                                    : false;

            this.PlaceID = Application.Current.Properties.ContainsKey(PlaceIDKey)
                                    ? (string)Application.Current.Properties[PlaceIDKey]
                                    : string.Empty;

            this.Address = Application.Current.Properties.ContainsKey(AddressKey)
                                    ? (string)Application.Current.Properties[AddressKey]
                                    : string.Empty;

            this.Latitude = Application.Current.Properties.ContainsKey(LatitudeKey)
                                    ? (double)Application.Current.Properties[LatitudeKey]
                                    : 0;

            this.Longitude = Application.Current.Properties.ContainsKey(LongitudeKey)
                                    ? (double)Application.Current.Properties[LongitudeKey]
                                    : 0;

            this.ProfileSynchronised = false;

            this.MenuListFetched = false;
        }

        public void SetLogin(bool logged)
        {
            this.HasLoggedIn = logged;
            Application.Current.Properties[LoggedInKey] = this.HasLoggedIn;

            if (!logged)
            {
                this.RemoveUser();
            }

            Application.Current.SavePropertiesAsync();
        }

        public Owner GetUser()
        {
            Owner owner = new Owner()
            {
                ID = this.ID,
                Username = this.Username,
                Password = this.Password,
                Email = this.Email,
                Name = this.Name,
                Headline = this.Headline,
                OpeningHour = this.OpeningHour,
                ClosingHour = this.ClosingHour,
                ProfilePicture = this.ProfilePicture,
                ProfilePictureThumb = this.ProfilePictureThumb,
                ContactNumber = this.ContactNumber,
                ContactWA = this.ContactWA,
                IsMaemseller = this.IsMaemseller,
                Location = new Location()
                {
                    Place_ID = this.PlaceID,
                    Address = this.Address,
                    Latitude = this.Latitude,
                    Longitude = this.Longitude
                }
            };

            return owner;
        }

        public void SetUser(Owner user)
        {
            this.ID = user.ID;
            Application.Current.Properties[IDKey] = this.ID;

            this.Username = user.Username;
            Application.Current.Properties[UsernameKey] = this.Username;

            this.Password = user.Password ?? this.Password;
            Application.Current.Properties[PasswordKey] = this.Password;

            this.Email = user.Email;
            Application.Current.Properties[EmailKey] = this.Email;

            this.Name = user.Name;
            Application.Current.Properties[NameKey] = this.Name;

            this.Headline = user.Headline;
            Application.Current.Properties[HeadlineKey] = this.Headline;

            this.OpeningHour = user.OpeningHour.ToString();
            Application.Current.Properties[OpeningHourKey] = this.OpeningHour;

            this.ClosingHour = user.ClosingHour.ToString();
            Application.Current.Properties[ClosingHourKey] = this.ClosingHour;

            this.ProfilePicture = user.ProfilePicture;
            Application.Current.Properties[ProfilePictureKey] = this.ProfilePicture;

            this.ProfilePictureThumb = user.ProfilePictureThumb;
            Application.Current.Properties[ProfilePictureThumbKey] = this.ProfilePictureThumb;

            this.ContactNumber = user.ContactNumber;
            Application.Current.Properties[ContactNumberKey] = this.ContactNumber;

            this.ContactWA = user.ContactWA;
            Application.Current.Properties[ContactWAKey] = this.ContactWA;

            this.IsMaemseller = user.IsMaemseller;
            Application.Current.Properties[IsMaemsellerKey] = this.IsMaemseller;

            this.PlaceID = user.Location?.Place_ID;
            Application.Current.Properties[PlaceIDKey] = this.PlaceID;

            this.Address = user.Location?.Address;
            Application.Current.Properties[AddressKey] = this.Address;

            this.Latitude = (user.Location != null) ? user.Location.Latitude : 0;
            Application.Current.Properties[LatitudeKey] = this.Latitude;

            this.Longitude = (user.Location != null) ? user.Location.Longitude : 0;
            Application.Current.Properties[LongitudeKey] = this.Longitude;

            Application.Current.SavePropertiesAsync();
        }

        public void SetMenuList(IList<Models.Menu> menuList)
        {
            this.MenuList = new ObservableCollection<Models.Menu>(menuList);
        }

        private void RemoveUser()
        {
            this.ID = string.Empty;
            if (Application.Current.Properties.ContainsKey(IDKey))
            {
                Application.Current.Properties.Remove(IDKey);
            }

            this.Username = string.Empty;
            if (Application.Current.Properties.ContainsKey(UsernameKey))
            {
                Application.Current.Properties.Remove(UsernameKey);
            }

            this.Password = string.Empty;
            if (Application.Current.Properties.ContainsKey(PasswordKey))
            {
                Application.Current.Properties.Remove(PasswordKey);
            }

            this.Email = string.Empty;
            if (Application.Current.Properties.ContainsKey(EmailKey))
            {
                Application.Current.Properties.Remove(EmailKey);
            }

            this.Name = string.Empty;
            if (Application.Current.Properties.ContainsKey(NameKey))
            {
                Application.Current.Properties.Remove(NameKey);
            }

            this.Headline = string.Empty;
            if (Application.Current.Properties.ContainsKey(HeadlineKey))
            {
                Application.Current.Properties.Remove(HeadlineKey);
            }

            this.OpeningHour = string.Empty;
            if (Application.Current.Properties.ContainsKey(OpeningHourKey))
            {
                Application.Current.Properties.Remove(OpeningHourKey);
            }

            this.ClosingHour = string.Empty;
            if (Application.Current.Properties.ContainsKey(ClosingHourKey))
            {
                Application.Current.Properties.Remove(ClosingHourKey);
            }

            this.ProfilePicture = string.Empty;
            if (Application.Current.Properties.ContainsKey(ProfilePictureKey))
            {
                Application.Current.Properties.Remove(ProfilePictureKey);
            }

            this.ProfilePictureThumb = string.Empty;
            if (Application.Current.Properties.ContainsKey(ProfilePictureThumbKey))
            {
                Application.Current.Properties.Remove(ProfilePictureThumbKey);
            }

            this.ContactNumber = string.Empty;
            if (Application.Current.Properties.ContainsKey(ContactNumberKey))
            {
                Application.Current.Properties.Remove(ContactNumberKey);
            }

            this.ContactWA = string.Empty;
            if (Application.Current.Properties.ContainsKey(ContactWAKey))
            {
                Application.Current.Properties.Remove(ContactWAKey);
            }

            this.IsMaemseller = false;
            if (Application.Current.Properties.ContainsKey(IsMaemsellerKey))
            {
                Application.Current.Properties.Remove(IsMaemsellerKey);
            }

            this.PlaceID = string.Empty;
            if (Application.Current.Properties.ContainsKey(PlaceIDKey))
            {
                Application.Current.Properties.Remove(PlaceIDKey);
            }

            this.Address = string.Empty;
            if (Application.Current.Properties.ContainsKey(AddressKey))
            {
                Application.Current.Properties.Remove(AddressKey);
            }

            this.Latitude = 0;
            if (Application.Current.Properties.ContainsKey(LatitudeKey))
            {
                Application.Current.Properties.Remove(LatitudeKey);
            }

            this.Longitude = 0;
            if (Application.Current.Properties.ContainsKey(LongitudeKey))
            {
                Application.Current.Properties.Remove(LongitudeKey);
            }

            Application.Current.SavePropertiesAsync();
        }
    }
}
