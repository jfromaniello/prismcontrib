using System;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.DSA.MenuServiceClient
{
    public class Callback : CallbackBase
    {
        public static event EventHandler<DataEventArgs<Restaurant[]>> GetRestaurantsReturn;
        public static event EventHandler<DataEventArgs<Exception>> GetRestaurantsException;
        public static event EventHandler<DataEventArgs<MenuItem[]>> GetMenuItemsReturn;
        public static event EventHandler<DataEventArgs<Exception>> GetMenuItemsException;

        #region GetMenuItems

        public override void OnGetMenuItemsReturn(Request request, object[] parameters, MenuItem[] returnValue)
        {
            if (GetMenuItemsReturn != null)
            {
                GetMenuItemsReturn(this, new DataEventArgs<MenuItem[]>(returnValue));
            }
        }

        public override OnExceptionAction OnGetMenuItemsException(Request request, Exception ex)
        {
            if (GetMenuItemsException != null)
            {
                GetMenuItemsException(this, new DataEventArgs<Exception>(ex));
            }
            return OnExceptionAction.Retry;
        }

        #endregion GetMenuItems

        #region GetRestaurants

        public override void OnGetRestaurantsReturn(Request request, object[] parameters, Restaurant[] returnValue)
        {
            if (GetRestaurantsReturn != null)
            {
                GetRestaurantsReturn(this, new DataEventArgs<Restaurant[]>(returnValue));
            }
        }

        public override OnExceptionAction OnGetRestaurantsException(Request request, Exception ex)
        {
            if (GetRestaurantsException != null)
            {
                GetRestaurantsException(this, new DataEventArgs<Exception>(ex));
            }
            return OnExceptionAction.Retry;
        }

        #endregion GetRestaurants

    }
}