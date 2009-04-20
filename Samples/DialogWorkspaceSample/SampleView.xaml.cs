using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogWorkspaceSample
{
    /// <summary>
    /// Interaction logic for SampleView.xaml
    /// </summary>
    public partial class SampleView : UserControl, ISampleView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleView"/> class.
        /// </summary>
        public SampleView()
        {
            InitializeComponent();
        }

        #region ISampleView Members

        /// <summary>
        /// Gets the first name that was provided
        /// </summary>
        /// <value></value>
        public string FirstName
        {
            get { return FirstNameTextBox.Text; }
            set { FirstNameTextBox.Text = value; }
        }

        /// <summary>
        /// Gets the last name that was provided
        /// </summary>
        /// <value></value>
        public string LastName
        {
            get { return LastNameTextBox.Text; }
            set { LastNameTextBox.Text = value; }
        }

        #endregion
    }
}
