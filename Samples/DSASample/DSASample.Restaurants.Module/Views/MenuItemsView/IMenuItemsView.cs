using System.Collections.Generic;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.Views.MenuItemsView
{
    public interface IMenuItemsView
    {
        void ShowMenuItems(string restaurantName);
        void ShowOfflineMessage();
        void ShowConnectionError();

        IEnumerable<MenuItem> Model { get; set; }
    }
}