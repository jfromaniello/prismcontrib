using System;
using DSASample.Infrastructure.Events;
using DSASample.Restaurants.Module.DSA.MenuServiceClient;
using DSASample.Restaurants.Module.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.Views.MenuItemsView
{
    public class MenuItemsViewPresenter
    {
        private Restaurant _restaurant;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionMonitor _connectionMonitor;
        private readonly Agent _restaurantService;

        public IMenuItemsView View
        { get; set; }

        private bool Connected
        {
            get { return _connectionMonitor.IsConnected; }
        }

        public MenuItemsViewPresenter(Agent restaurantService, IConnectionMonitor connectionMonitor, IEventAggregator eventAggregator)
        {
            _restaurantService = restaurantService;
            _connectionMonitor = connectionMonitor;
            _eventAggregator = eventAggregator;

            RestaurantSelectedEvent restaurantSelectedEvent = _eventAggregator.GetEvent<RestaurantSelectedEvent>();
            restaurantSelectedEvent.Subscribe(RestaurantSelected, ThreadOption.UIThread, true);

            // Subscribe to the service agent callback events
            Callback.GetMenuItemsReturn += GetMenuItemsReturn;
            Callback.GetMenuItemsException += GetMenuItemsException;
        }

        public void TryGetMenuItems()
        {
            if (Connected)
            {
                OnProcessing(2);
                OnStatusUpdate("Retrieving menu items...");
                _restaurantService.GetMenuItems(_restaurant.Identifier);
            }
            else
            {
                // If the application is not online, update the view to show an offline message
                View.ShowOfflineMessage();
            }
        }

        public void RestaurantSelected(Restaurant restaurant)
        {
            _restaurant = restaurant;
            TryGetMenuItems();
        }

        private void OnStatusUpdate(string message)
        {
            StatusUpdateEvent statusUpdateEvent = _eventAggregator.GetEvent<StatusUpdateEvent>();
            statusUpdateEvent.Publish(message);
        }

        private void OnProcessing(int seconds)
        {
            ProcessingEvent processingEvent = _eventAggregator.GetEvent<ProcessingEvent>();
            processingEvent.Publish(seconds);
        }

        /// <summary>
        /// This method will be called when the service agent gets a
        /// successful response from the GetMenuItems method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GetMenuItems return value</param>
        private void GetMenuItemsReturn(object sender, DataEventArgs<MenuItem[]> e)
        {
            OnProcessing(2);
            OnStatusUpdate("Menu items recieved.");
            View.Model = e.Value;
            View.ShowMenuItems(_restaurant.Name);
        }

        /// <summary>
        /// This method will be called when the service agent gets an
        /// exception when calling the GetMenuItems method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Exception thrown</param>
        private void GetMenuItemsException(object sender, DataEventArgs<Exception> e)
        {
            OnProcessing(2);
            if (Connected)
            {
                OnStatusUpdate("An error has ocurred while retrieving the menu items.");
                View.ShowConnectionError();
            }
            else
            {
                View.ShowOfflineMessage();
            }
        }
    }
}