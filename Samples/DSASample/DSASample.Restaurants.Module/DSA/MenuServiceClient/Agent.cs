using System;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.DSA.MenuServiceClient
{
    public partial class Agent
    {
        IRequestQueue requestQueue;

        public Agent(IRequestQueue requestQueue)
        {
            this.requestQueue = requestQueue;
        }

        #region GetMenuItems

        /// <summary>
        /// Enqueues a request to the <c>GetMenuItems</c> web service method through the agent.
        /// </summary>
        /// <returns>The unique identifier associated with the request that was enqueued.</returns>
        public Guid GetMenuItems(String restaurantId)
        {
            return GetMenuItems(restaurantId, GetGetMenuItemsDefaultBehavior());
        }

        /// <summary>
        /// Enqueues a request to the <c>GetMenuItems</c> web service method through the agent.
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="behavior">The behavior associated with the offline request being enqueued.</param>
        /// <returns>The unique identifier associated with the request that was enqueued.</returns>
        public Guid GetMenuItems(String restaurantId, OfflineBehavior behavior)
        {
            behavior.ReturnCallback = new CommandCallback(typeof(Callback), "OnGetMenuItemsReturn");
            behavior.ExceptionCallback = new CommandCallback(typeof(Callback), "OnGetMenuItemsException");

            return EnqueueRequest("GetMenuItems", behavior, restaurantId);
        }

        public static OfflineBehavior GetGetMenuItemsDefaultBehavior()
        {
            OfflineBehavior behavior = GetAgentDefaultBehavior();
            behavior.ReturnCallback = new CommandCallback(typeof(Callback), "OnGetMenuItemsReturn");
            behavior.ExceptionCallback = new CommandCallback(typeof(Callback), "OnGetMenuItemsException");

            return behavior;
        }

        #endregion GetMenuItems

        #region GetRestaurants

        /// <summary>
        /// Enqueues a request to the <c>GetRestaurants</c> web service method through the agent.
        /// </summary>
        /// <returns>The unique identifier associated with the request that was enqueued.</returns>
        public Guid GetRestaurants()
        {
            return GetRestaurants(GetGetRestaurantsDefaultBehavior());
        }

        /// <summary>
        /// Enqueues a request to the <c>GetRestaurants</c> web service method through the agent.
        /// </summary>
        /// <param name="behavior">The behavior associated with the offline request being enqueued.</param>
        /// <returns>The unique identifier associated with the request that was enqueued.</returns>
        public Guid GetRestaurants(OfflineBehavior behavior)
        {
            behavior.ReturnCallback = new CommandCallback(typeof(Callback), "OnGetRestaurantsReturn");
            behavior.ExceptionCallback = new CommandCallback(typeof(Callback), "OnGetRestaurantsException");

            return EnqueueRequest("GetRestaurants", behavior);
        }

        public static OfflineBehavior GetGetRestaurantsDefaultBehavior()
        {
            OfflineBehavior behavior = GetAgentDefaultBehavior();
            behavior.ReturnCallback = new CommandCallback(typeof(Callback), "OnGetRestaurantsReturn");
            behavior.ExceptionCallback = new CommandCallback(typeof(Callback), "OnGetRestaurantsException");

            return behavior;
        }

        #endregion GetRestaurants

        #region Common

        public static OfflineBehavior GetAgentDefaultBehavior()
        {
            OfflineBehavior behavior = new OfflineBehavior();
            behavior.MaxRetries = 3;
            behavior.Stamps = 1;
            behavior.Expiration = DateTime.Now + new TimeSpan(24, 0, 0, 0);
            behavior.ProxyFactoryType = typeof(WCFProxyFactory<IMenuService>);

            return behavior;
        }

        private Guid EnqueueRequest(string methodName, OfflineBehavior behavior, params object[] arguments)
        {
            Request request = CreateRequest(methodName, behavior, arguments);

            requestQueue.Enqueue(request);

            return request.RequestId;
        }

        private static Request CreateRequest(string methodName, OfflineBehavior behavior, params object[] arguments)
        {
            Request request = new Request();
            request.MethodName = methodName;
            request.Behavior = behavior;
            request.CallParameters = arguments;

            request.OnlineProxyType = OnlineProxyType;
            request.Endpoint = Endpoint;

            return request;
        }

        public static Type OnlineProxyType
        {
            get
            {
                return typeof(global::RestaurantModule.MenuService.MenuServiceClient);
            }
        }

        public static string Endpoint
        {
            get
            {
                return "";
            }
        }
        #endregion
    }
}