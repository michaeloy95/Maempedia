using Maempedia.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maempedia.Data
{
    public class LocalDatabaseSavedMenu
    {
        private readonly SQLiteAsyncConnection database;

        public LocalDatabaseSavedMenu(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<LocalMenu>().Wait();
        }

        public async Task<List<LocalMenu>> GetMenusAsync()
        {
            return await database.Table<LocalMenu>().ToListAsync();
        }

        public async Task<LocalMenu> GetMenuAsync(string menuId)
        {
            return await database.Table<LocalMenu>().Where(menu => menu.ID == menuId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveMenuAsync(LocalMenu localMenu)
        {
            var m = await database.Table<LocalMenu>().Where(menu => menu.ID == localMenu.ID).FirstOrDefaultAsync();

            if (m != null)
            {
                return await database.UpdateAsync(localMenu);
            }
            else
            {
                return await database.InsertAsync(localMenu);
            }
        }

        public async Task<int> DeleteMenuAsync(LocalMenu localMenu)
        {
            return await database.DeleteAsync(localMenu);
        }
    }
}
