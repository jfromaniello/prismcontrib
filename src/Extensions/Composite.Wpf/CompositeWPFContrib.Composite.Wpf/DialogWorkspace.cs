using System;
using System.Windows;
using CompositeWPFContrib.Composite.Wpf.Controls;

namespace CompositeWPFContrib.Composite.Wpf
{
    /// <summary>
    /// Provides a separate workspace for modal dialogs.
    /// </summary>
    public static class DialogWorkspace
    {
        private static ViewDialog _currentDialog;

        /// <summary>
        /// Displays a view in the dialog workspace
        /// and returns the dialog result after the dialog has been closed
        /// </summary>
        /// <param name="view">The view to display</param>
        /// <returns>Returns the dialog result of the dialog</returns>
        public static bool? ShowView(object view)
        {
            return ShowView(view, null);
        }

        /// <summary>
        /// Displays a view in the dialog workspace
        /// and returns the dialog result after the dialog has been closed
        /// </summary>
        /// <param name="view">The view to display</param>
        /// <param name="validationAction">Validation function for the view</param>
        /// <returns>Returns the dialog result of the dialog</returns>
        public static bool? ShowView(object view, Func<bool> validationAction)
        {
            return ShowView(view, null, validationAction);
        }

        /// <summary>
        /// Closes the active dialog without returning a result
        /// </summary>
        public static void CloseActiveWorkspace()
        {
            if (_currentDialog != null)
            {
                _currentDialog.DialogResult = null;
                _currentDialog.Close();
            }
        }

        /// <summary>
        /// Displays a view in the dialog workspace
        /// and returns the dialog result after the dialog has been closed
        /// </summary>
        /// <param name="view">The view to display</param>
        /// <param name="options">Options for the dialog</param>
        /// <param name="validationAction">Validation function for the view</param>
        /// <returns>Returns the dialog result of the dialog</returns>
        public static bool? ShowView(object view, DialogOptions options,Func<bool> validationAction)
        {
            ViewDialog dialog = new ViewDialog();
            dialog.View = view as DependencyObject;

            // Apply the dialog options if provided
            if (options != null)
            {
                dialog.ResizeMode = options.AllowResize ? ResizeMode.CanResize: ResizeMode.NoResize;
                dialog.Title = options.DialogTitle ?? "";
                dialog.ShowInTaskbar = options.ShowInTaskBar;

                if (options.HideOkCancelButtons)
                {
                    dialog.ButtonPanelVisibility = Visibility.Collapsed;
                }

                if (options.DialogSize.Width > 0 && options.DialogSize.Height > 0)
                {
                    dialog.Width = options.DialogSize.Width;
                    dialog.Height = options.DialogSize.Height;
                    dialog.SizeToContent = SizeToContent.Manual;
                }
            }

            dialog.ValidateDialog += new EventHandler<System.ComponentModel.CancelEventArgs>((sender, e) =>
            {
                e.Cancel = false;

                if (validationAction != null)
                {
                    // Validate action will return false when validation fails
                    // invert this result to cancel the close action
                    e.Cancel = !validationAction();
                }
            });

            _currentDialog = dialog;

            bool? result = dialog.ShowDialog();

            _currentDialog = null;

            return result;
        }
    }
}
