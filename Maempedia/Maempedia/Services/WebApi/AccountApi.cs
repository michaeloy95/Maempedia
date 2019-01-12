using Maempedia.Enum;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maempedia.Services.WebApi
{
    public class AccountApi : ApiRequest
    {
        public async Task<ServerResponseStatus> CheckUsernameIsValid(string username) =>
            await CheckAccountData($"check_username.html?username={username}");

        public async Task<ServerResponseStatus> CheckEmailIsValid(string email) =>
            await CheckAccountData($"check_email.html?email={email}");

        public async Task<ServerResponseStatus> CheckNameIsValid(string name) =>
            await CheckAccountData($"check_name.html?name={name}");

        public async Task<ServerResponseStatus> CheckContactIsValid(string contact) =>
            await CheckAccountData($"check_contact.html?contact={contact}");

        public async Task<ServerResponseStatus> TryLogin(string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"login.html", values);

            if (data == null)
            {
                return ServerResponseStatus.ERROR;
            }
            else if (data["status"].ToString() == "DENIED")
            {
                return ServerResponseStatus.INVALID;
            }

            var owner = new Models.Owner((JObject)data["user"]);

            App.User.SetUser(owner);
            App.User.ProfileSynchronised = true;

            return ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> TryRegister(string username, string password, string email, string contact)
        {
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "email", email },
                { "wacontact", contact },
                { "name", username }
            };
            var data = await this.PostToMaempedia($"register.html", values);

            if (data == null)
            {
                return ServerResponseStatus.ERROR;
            }
            else if (data["status"].ToString() == "DENIED")
            {
                return ServerResponseStatus.INVALID;
            }

            var owner = new Models.Owner((JObject)data["owner"]);
            
            App.User.SetUser(owner);
            App.User.ProfileSynchronised = true;

            return ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> UpdateAccount(Models.Owner owner, byte[] image = null)
        {
            var values = new Dictionary<string, string>
            {
                { "username", owner.Username },
                { "password", owner.Password },
                { "email", owner.Email },
                { "name", owner.Name },
                { "opening_hour", owner.OpeningHour },
                { "closing_hour", owner.ClosingHour },
                { "description", owner.Headline },
                { "location_address", owner.Location.Address },
                { "location_lat", owner.Location.Latitude.ToString() },
                { "location_lng", owner.Location.Longitude.ToString() },
                { "wacontact", owner.ContactWA },
                { "contact", owner.ContactNumber },
                { "is_maemseller", owner.IsMaemseller.ToString() }
            };
            var data = await this.PostToMaempedia($"update_account.html?id={owner.ID}", values);

            if (data == null)
            {
                return ServerResponseStatus.ERROR;
            }
            else if (data["status"].ToString() == "DENIED")
            {
                return ServerResponseStatus.INVALID;
            }

            return image == null
                ? ServerResponseStatus.VALID
                : await this.UploadImage(image, owner.ID);
        }

        public async Task<ServerResponseStatus> UploadImage(byte[] image, string id)
        {
            var data = await this.PostMediaToMaempedia($"owner_image.html", image, id);

            return data == null ? ServerResponseStatus.ERROR
                : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
                : ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> SendFeedback(string id, string comment, string suggestion)
        {
            var values = new Dictionary<string, string>
            {
                { "comment", comment },
                { "suggestion", suggestion }
            };
            var data = await this.PostToMaempedia($"feedbacks.html?id={id}", values);

            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }

        public async Task<ServerResponseStatus> ReportBugs(string id, string comment)
        {
            var values = new Dictionary<string, string>
            {
                { "comment", comment }
            };
            var data = await this.PostToMaempedia($"bugs_report.html?id={id}", values);

            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }

        private async Task<ServerResponseStatus> CheckAccountData(string endpoint)
        {
            var data = await this.GetFromMaempedia(endpoint);
            if (data == null)
            {
                return ServerResponseStatus.ERROR;
            }

            return data["status"].ToString() == "VALID"
                ? ServerResponseStatus.VALID
                : ServerResponseStatus.INVALID;
        }
    }
}
