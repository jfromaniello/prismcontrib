using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RestaurantModule.MenuService;

namespace DSASample.Restaurants.Module.Views.RestaurantsView
{
    /// <summary>
    /// Interaction logic for RestaurantsView.xaml
    /// </summary>
    public partial class RestaurantsView : UserControl, IRestaurantsView
    {
        private readonly RestaurantsViewPresenter _presenter;

        public RestaurantsView()
        {
            InitializeComponent();

            this.btn_retry.Click += btn_retry_Click;
            this.menuItemlistView.SelectionChanged += menuItemlistView_SelectionChanged;
        }

        public RestaurantsView(RestaurantsViewPresenter presenter)
            : this()
        {
            _presenter = presenter;
            _presenter.View = this;

            _presenter.TryGetRestaurants();
        }

        #region IRestaurantsView Members

        public IEnumerable<Restaurant> Model
        {
            get { return this.menuItemlistView.DataContext as IEnumerable<Restaurant>; }
            set
            {
                if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
                {
                    this.menuItemlistView.DataContext = value;
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate { this.menuItemlistView.DataContext = value; });
                }
            }
        }

        private delegate void ShowRestaurantsDelegate();
        public void ShowRestaurants()
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                menuItemlistView.Visibility = Visibility.Visible;
                txt_offline.Visibility = Visibility.Collapsed;
                txt_error.Visibility = Visibility.Collapsed;
                btn_retry.Visibility = Visibility.Collapsed;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowRestaurantsDelegate(ShowRestaurants));
            }
        }

        private delegate void ShowMessageDelegate();
        public void ShowOfflineMessage()
        {
            if (this.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                menuItemlistView.Visibility = Visibility.Collapsed;
                txt_offline.Visibility = Visibility.Visible;
                txt_error.Visibility = Visibility.Collapsed;
                btn_retry.Visibility = Visibility.Collapsed;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowMessageDelegate(ShowOfflineMessage));
            }
        }

        public void ShowConnectionError()
        {
            if (this.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                txt_error.Visibility = Visibility.Visible;
                btn_retry.Visibility = Visibility.Visible;
                menuItemlistView.Visibility = Visibility.Collapsed;
                txt_offline.Visibility = Visibility.Collapsed;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowMessageDelegate(ShowConnectionError));
            }
        }

        #endregion

        private void btn_retry_Click(object sender, RoutedEventArgs e)
        {
            _presenter.TryGetRestaurants();
        }

        private void menuItemlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menuItemlistView.SelectedItems.Count > 0)
            {
                _presenter.RestaurantSelected((Restaurant)menuItemlistView.SelectedItem);
            }
        }
    }
}