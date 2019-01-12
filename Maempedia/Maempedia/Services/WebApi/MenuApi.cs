using Maempedia.Enum;
using Maempedia.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maempedia.Services.WebApi
{
    public class MenuApi : ApiRequest
    {
        public async Task<IList<Menu>> GetMenus(int start, SortMenuBy sortBy, string keyword, double lat = 0, double lng = 0)
        {
            const int count = 5;
            string type = sortBy == SortMenuBy.Nearby ? "Terdekat"
                : sortBy == SortMenuBy.Trending ? "Trending"
                : sortBy == SortMenuBy.Latest ? "Terbaru"
                : string.Empty;

            var uri = $"menus.html?type={type}&start={start}&count={count}&keyword={keyword}";
            if (lat == 0 && lng == 0)
            {
                uri += $"&lat={lat}&lng={lng}";
            }

            var data = await this.GetFromMaempedia(uri);
            if (data == null)
            {
                return null;
            }

            var menuList = new List<Menu>();
            foreach (JObject menu in data["menu"])
            {
                menuList.Add(new Menu(menu));
            }

            return menuList;
        }

        public async Task<IList<Menu>> GetMenusFromOwnerId(Owner owner)
        {
            var data = await this.GetFromMaempedia($"menus.html?user_id={owner.ID}");
            if (data == null)
            {
                return null;
            }

            var menuList = new List<Menu>();
            foreach (JObject menu in data["menu"])
            {
                menuList.Add(new Menu(menu));
            }

            return menuList;
        }

        public async Task<Menu> GetMenu(string menuId)
        {
            var data = await this.GetFromMaempedia($"menus.html?menu_id={menuId}");
            if (data == null)
            {
                return null;
            }

            return new Menu(data);
        }

        public async Task<ServerResponseStatus> AddMenu(Menu menu, byte[] image, string ownerId, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "name", menu.Name },
                { "description", menu.Headline },
                { "portion", menu.Portion },
                { "price", menu.Price.ToString() },
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"add_menu.html?owner_id={ownerId}", values);

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
                : await this.UploadImage(image, menu.ID);
        }

        public async Task<ServerResponseStatus> UpdateMenu(Menu menu, byte[] image, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "name", menu.Name },
                { "description", menu.Headline },
                { "portion", menu.Portion },
                { "price", menu.Price.ToString() },
                { "like", menu.Like.ToString() },
                { "promoted ", menu.Promoted.ToString() },
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"update_menu.html?id={menu.ID}", values);

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
                : await this.UploadImage(image, menu.ID);
        }

        public async Task<ServerResponseStatus> DeleteMenu(string id, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"delete_menu.html?id={id}", values);

            return data == null ? ServerResponseStatus.ERROR
               : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
               : ServerResponseStatus.VALID;
        }

        private async Task<ServerResponseStatus> UploadImage(byte[] image, string id)
        {
            var data = await this.PostMediaToMaempedia($"menu_image.html", image, id);

            return data == null ? ServerResponseStatus.ERROR
                : data["status"].ToString() == "DENIED" ? ServerResponseStatus.INVALID
                : ServerResponseStatus.VALID;
        }

        public async Task<Tuple<ServerResponseStatus, string>> RequestPromotion(string id, int price, int days_count, string voucher_code, string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "price", price.ToString() },
                { "days_count", days_count.ToString() },
                { "voucher_code", voucher_code },
                { "username", username },
                { "password", password }
            };
            var data = await this.PostToMaempedia($"set_ads.html?id={id}", values);

            return data == null ? Tuple.Create(ServerResponseStatus.ERROR, string.Empty)
                : data["status"].ToString() == "DENIED" ? Tuple.Create(ServerResponseStatus.INVALID, string.Empty)
                : Tuple.Create(ServerResponseStatus.VALID, data["referencecode"].ToString());
        }
    }
}
