using System;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.DSA.MenuServiceClient
{
    public abstract class CallbackBase
    {
        #region GetMenuItems

        public abstract void OnGetMenuItemsReturn(Request request, object[] parameters, MenuItem[] returnValue);

        public abstract OnExceptionAction OnGetMenuItemsException(Request request, Exception ex);

        #endregion GetMenuItems

        #region GetRestaurants

        public abstract void OnGetRestaurantsReturn(Request request, object[] parameters, Restaurant[] returnValue);

        public abstract OnExceptionAction OnGetRestaurantsException(Request request, Exception ex);

        #endregion GetRestaurants

    }
}