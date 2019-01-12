using Maempedia.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maempedia.Services.WebApi
{
    public class OwnerApi : ApiRequest
    {
        public async Task<IList<Owner>> GetNearbyOwners(double lat, double lng, int max)
        {
            var data = await this.GetFromMaempedia($"owners.html?lat={lat}&lng={lng}&max={max}");
            if (data == null)
            {
                return null;
            }

            var ownerList = new List<Owner>();
            foreach (JObject owner in data["owner"])
            {
                ownerList.Add(new Owner(owner));
            }

            return ownerList;
        }

        public async Task<Owner> GetOwner(string ownerId)
        {
            var data = await this.GetFromMaempedia($"owner_details.html?id={ownerId}");
            if (data == null)
            {
                return null;
            }

            return new Owner(data);
        }
    }
}
