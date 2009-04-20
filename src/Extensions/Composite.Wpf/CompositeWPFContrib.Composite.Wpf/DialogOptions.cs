using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CompositeWPFContrib.Composite.Wpf
{
    /// <summary>
    /// Contains various settings for dialogs
    /// </summary>
    public class DialogOptions
    {
        /// <summary>
        /// Gets or sets the title to display on the dialog
        /// </summary>
        public string DialogTitle { get; set; }

        /// <summary>
        /// Gets or sets whether the dialog should be displayed in the Windows taskbar
        /// </summary>
        public bool ShowInTaskBar { get; set; }

        /// <summary>
        /// Gets or sets whether users should be able to resize the dialog
        /// </summary>
        public bool AllowResize { get; set; }

        /// <summary>
        /// Gets or sets whether the Ok and cancel button should be hidden
        /// </summary>
        public bool HideOkCancelButtons { get; set; }

        /// <summary>
        /// Gets or sets the size of the dialog
        /// </summary>
        /// <remarks>Leave empty to enable auto size</remarks>
        public Size DialogSize { get; set; }
    }
}
