using System.Collections.Generic;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.Views.RestaurantsView
{
    public interface IRestaurantsView
    {
        void ShowRestaurants();
        void ShowOfflineMessage();
        void ShowConnectionError();

        IEnumerable<Restaurant> Model { get; set; }
    }
}