using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Magentix.QLicense;
using Magentix.License;
using System.Reflection;

namespace Magentix.Presentation
{
    /// <summary>
    /// Interaction logic for LicenseActivationWindow.xaml
    /// </summary>
    public partial class LicenseActivationWindow : Window
    {
        public LicenseActivationWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtActivationCode.Text))
            {
                MessageBox.Show("Please input license", "MagentixPOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Check the activation string
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            string _msg = string.Empty;
            byte[] CertificatePublicKeyData;
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                Stream stream = _assembly.GetManifestResourceStream("Magentix.Presentation.LicenseVerify.cer");
                stream.CopyTo(_mem);

                CertificatePublicKeyData = _mem.ToArray();
            }
            MagentixLicense _lic = (MagentixLicense)LicenseHandler.ParseLicenseFromBASE64String(typeof(MagentixLicense), txtActivationCode.Text.Trim(), CertificatePublicKeyData, out _licStatus, out _msg);
            switch (_licStatus)
            {
                case LicenseStatus.VALID:
                    if (_lic.TrialVersion == true && _lic.TrialDate < DateTime.Now)
                    {
                        MessageBox.Show("This key is expired. Please buy full version key.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtActivationCode.Select(0, txtActivationCode.Text.Length);
                        txtActivationCode.Focus();
                        return;
                    }
                    File.WriteAllText("license.lic", txtActivationCode.Text);
                    this.DialogResult = true;
                    return;

                case LicenseStatus.CRACKED:
                case LicenseStatus.INVALID:
                case LicenseStatus.UNDEFINED:
                    MessageBox.Show("License is INVALID", "MagentixPOS", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                default:
                    return;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            String AppName = "MagentixPOS";
            txtDeviceCode.Text = LicenseHandler.GenerateUID(AppName);
        }
    }
}
