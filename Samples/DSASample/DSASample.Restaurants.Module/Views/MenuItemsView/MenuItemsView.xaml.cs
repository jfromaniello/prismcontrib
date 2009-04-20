using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DSASample.Restaurants.Module.Views.MenuItemsView
{
    /// <summary>
    /// Interaction logic for MenuItemsView.xaml
    /// </summary>
    public partial class MenuItemsView : UserControl, IMenuItemsView
    {
        private readonly MenuItemsViewPresenter _presenter;

        public MenuItemsView()
        {
            InitializeComponent();
            this.btn_retry.Click += btn_retry_Click;
        }

        public MenuItemsView(MenuItemsViewPresenter presenter)
            : this()
        {
            _presenter = presenter;
            _presenter.View = this;
        }

        #region IMenuItemsView Members

        public IEnumerable<global::RestaurantModule.MenuService.MenuItem> Model
        {
            get { return this.menuItemlistView.DataContext as IEnumerable<global::RestaurantModule.MenuService.MenuItem>; }
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

        private delegate void ShowMenuItemsDelegate(string restaurantName);
        public void ShowMenuItems(string restaurantName)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                txt_restaurant.Text = String.Format(CultureInfo.CurrentCulture, "Menu items for \"{0}\" restaurant", restaurantName);

                txt_error.Visibility = Visibility.Collapsed;
                btn_retry.Visibility = Visibility.Collapsed;
                txt_offline.Visibility = Visibility.Collapsed;
                menuItemlistView.Visibility = Visibility.Visible;
                txt_restaurant.Visibility = Visibility.Visible;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowMenuItemsDelegate(ShowMenuItems), restaurantName);
            }
        }

        private delegate void ShowMessageDelegate();
        public void ShowOfflineMessage()
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                menuItemlistView.Visibility = Visibility.Collapsed;
                txt_offline.Visibility = Visibility.Visible;
                txt_error.Visibility = Visibility.Collapsed;
                btn_retry.Visibility = Visibility.Collapsed;
                txt_restaurant.Visibility = Visibility.Collapsed;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowMessageDelegate(ShowOfflineMessage));
            }
        }

        public void ShowConnectionError()
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                txt_error.Text = "There has been an error contacting the back-end service.";

                btn_retry.Visibility = Visibility.Visible;
                txt_error.Visibility = Visibility.Visible;
                menuItemlistView.Visibility = Visibility.Collapsed;
                txt_offline.Visibility = Visibility.Collapsed;
                txt_restaurant.Visibility = Visibility.Collapsed;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new ShowMessageDelegate(ShowConnectionError));
            }
        }

        #endregion

        private void btn_retry_Click(object sender, RoutedEventArgs e)
        {
            _presenter.TryGetMenuItems();
        }
    }
}