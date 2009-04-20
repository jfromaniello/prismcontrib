using System;
using DSASample.Infrastructure.Events;
using DSASample.Restaurants.Module.DSA.MenuServiceClient;
using DSASample.Restaurants.Module.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.SmartClient.DisconnectedAgent;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.Views.RestaurantsView
{
    public class RestaurantsViewPresenter
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionMonitor _connectionMonitor;
        private readonly Agent _restaurantService;

        public IRestaurantsView View
        { get; set; }

        private bool Connected
        {
            get { return _connectionMonitor.IsConnected; }
        }

        public RestaurantsViewPresenter(Agent restaurantService, IConnectionMonitor connectionMonitor, IEventAggregator eventAggregator)
        {
            _connectionMonitor = connectionMonitor;
            _restaurantService = restaurantService;
            _eventAggregator = eventAggregator;

            // Subscribe to the service agent callback events
            Callback.GetRestaurantsReturn += GetRestaurantsReturn;
            Callback.GetRestaurantsException += GetRestaurantsException;
        }

        public void TryGetRestaurants()
        {
            if (Connected)
            {
                OnProcessing(2);
                OnStatusUpdate("Retrieving restaurants list...");
            }
            else
            {
                // If the application is not online, update the view to show an offline message
                View.ShowOfflineMessage();
            }

            // Enqueue a request. The request will be dispatched when a connection is detected.
            _restaurantService.GetRestaurants();
        }

        public void RestaurantSelected(Restaurant restaurant)
        {
            RestaurantSelectedEvent restaurantSelectedEvent = _eventAggregator.GetEvent<RestaurantSelectedEvent>();
            restaurantSelectedEvent.Publish(restaurant);
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
        /// successful response from the GetRestaurants method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GetRestaurants return value</param>
        private void GetRestaurantsReturn(object sender, DataEventArgs<Restaurant[]> e)
        {
            OnProcessing(2);
            OnStatusUpdate("Restaurants list recieved.");
            View.Model = e.Value;
            View.ShowRestaurants();
        }

        /// <summary>
        /// This method will be called when the service agent gets an
        /// exception when calling the GetRestaurants method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Exception thrown</param>
        private void GetRestaurantsException(object sender, DataEventArgs<Exception> e)
        {
            OnProcessing(2);
            if (Connected)
            {
                OnStatusUpdate("An error has ocurred while retrieving the restaurants list.");
                View.ShowConnectionError();
            }
            else
            {
                View.ShowOfflineMessage();
            }
        }
    }
}