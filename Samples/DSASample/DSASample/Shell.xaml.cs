using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DSASample
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IShell
    {
        public Shell()
        {
            InitializeComponent();
        }

        #region IShell members

        private delegate void UpdateStatusMessageDelegate(string message);
        public void UpdateStatusMessage(string message)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                statusMessage.Text = message;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new UpdateStatusMessageDelegate(UpdateStatusMessage), message);
            }
        }

        public void ShowProcessing(int seconds)
        {
            progressBar.Value = 0.0;
            progressBar.Visibility = Visibility.Visible;

            Duration duration = new Duration(TimeSpan.FromSeconds(seconds));
            DoubleAnimation animation = new DoubleAnimation(100.0, duration);
            animation.Completed += OnAnimationCompleted;
            progressBar.BeginAnimation(RangeBase.ValueProperty, animation);
        }

        #endregion

        private void OnAnimationCompleted(object sender, EventArgs args)
        {
            progressBar.Visibility = Visibility.Hidden;
        }
    }
}