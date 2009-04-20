using System;
using System.Collections.Generic;
using RestaurantService.Host.DataContracts;
using RestaurantService.Host.DataSet;
using RestaurantService.Host.Translator;
using System.Data;
using System.Globalization;

namespace RestaurantService.Host
{
    public class MenuService : IMenuService
    {
        private readonly RestaurantDataSet _dataSet = RestaurantRepository.Instance;

        public MenuItem[] GetMenuItems(string restaurantId)
        {
            DataRow[] rows = _dataSet.MenuItem.Select(String.Format(CultureInfo.CurrentCulture, "Restaurant='{0}'", restaurantId));
            List<MenuItem> menuItems = new List<MenuItem>(rows.Length);

            foreach (RestaurantDataSet.MenuItemRow row in rows)
            {
                menuItems.Add(DataTranslator.TranslateFromMenuItemRowToMenuItemEntity(row));
            }

            return menuItems.ToArray();
        }

        public Restaurant[] GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            foreach (RestaurantDataSet.RestaurantRow row in _dataSet.Restaurant.Rows)
            {
                restaurants.Add(DataTranslator.TranslateFromRestaurantRowToRestaurantEntity(row));
            }

            return restaurants.ToArray();
        }
    }
}

