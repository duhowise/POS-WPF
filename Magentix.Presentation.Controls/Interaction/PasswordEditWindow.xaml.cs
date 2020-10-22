using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magentix.Presentation.Controls.Interaction
{
    /// <summary>
    /// Interaction logic for PasswordEditWindow.xaml
    /// </summary>
    public partial class PasswordEditWindow : Window
    {
        private readonly AdminPinProperties _properties;
        public string AdminPin
        {
            get
            {
                return this._properties.AdminPin;
            }
            set
            {
                this._properties.AdminPin = value;
            }
        }

        public PasswordEditWindow()
        {
            InitializeComponent();
            this._properties = new AdminPinProperties();
            base.DataContext = this._properties;
            this.MiddleRow.Height = new GridLength(4, GridUnitType.Star);
        }

        private void Edit_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                base.DialogResult = new bool?(true);
            }
        }

        private void PasswordEditWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (base.Width < base.Height)
            {
                this.MiddleColumn.Width = new GridLength(9, GridUnitType.Star);
                return;
            }
            if (this.MiddleColumn.Width.Value != 2)
            {
                this.MiddleColumn.Width = new GridLength(2, GridUnitType.Star);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Edit.Focus();
        }
    }
}
