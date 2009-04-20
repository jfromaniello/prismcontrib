using System.Windows;
using System;
using System.ComponentModel;

namespace CompositeWPFContrib.Composite.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ViewDialog.xaml
    /// </summary>
    public partial class ViewDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ViewDialog"/>
        /// </summary>
        public ViewDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets fired when the dialog needs to be validated
        /// </summary>
        public event EventHandler<CancelEventArgs> ValidateDialog;

        #region Event handlers

        /// <summary>
        /// Handles the click event of the cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the click event of the accept button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            EventHandler<CancelEventArgs> handler = ValidateDialog;
            bool cancelled = false;

            if (handler != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                handler(this, args);

                cancelled = args.Cancel;
            }

            if (!cancelled)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        #endregion

        #region ButtonPanelVisibility dependency property

        public Visibility ButtonPanelVisibility
        {
            get { return (Visibility)GetValue(ButtonPanelVisibilityProperty); }
            set { SetValue(ButtonPanelVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonPanelVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonPanelVisibilityProperty =
            DependencyProperty.Register("ButtonPanelVisibility", typeof(Visibility), typeof(ViewDialog), new UIPropertyMetadata(Visibility.Visible));

        #endregion

        #region View dependency property

        /// <summary>
        /// Gets or sets the view to show in the dialog
        /// </summary>
        public DependencyObject View
        {
            get { return (DependencyObject)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(DependencyObject), 
            typeof(ViewDialog), new UIPropertyMetadata(null));

        #endregion
    }
}
