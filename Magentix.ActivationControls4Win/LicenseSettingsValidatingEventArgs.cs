using System;

namespace Magentix.QLicense.Windows.Controls
{
    public class LicenseSettingsValidatingEventArgs:EventArgs
    {
        public LicenseEntity License { get; set; }
        public bool CancelGenerating { get; set; }
    }
}
