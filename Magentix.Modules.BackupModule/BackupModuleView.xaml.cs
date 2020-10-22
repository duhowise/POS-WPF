using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Magentix.Modules.BackupModule
{
    /// <summary>
    /// Interaction logic for MenuModuleView.xaml
    /// </summary>
    
    [Export]
    public partial class BackupModuleView : UserControl
    {
        [ImportingConstructor]
        public BackupModuleView(BackupModuleViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
