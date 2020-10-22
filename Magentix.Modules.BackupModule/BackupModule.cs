using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Regions;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Domain.Models.Settings;
using System;
using Magentix.Infrastructure.Settings;

namespace Magentix.Modules.BackupModule
{
    [ModuleExport(typeof(BackupModule))]
    public class BackupModule : VisibleModuleBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;
        private readonly BackupModuleView _BackupModuleView;
        private readonly BackupModuleViewModel _BackupModuleViewModel;
        private readonly BackupHelper _backupHelper;

        [ImportingConstructor]
        public BackupModule(IRegionManager regionManager, IUserService userService, BackupModuleView BackupModuleView, BackupModuleViewModel BackupModuleViewModel, BackupHelper backupHelper)
            : base(regionManager, AppScreens.BackupView)
        {
            _regionManager = regionManager;
            _userService = userService;
            _BackupModuleView = BackupModuleView;
            _BackupModuleViewModel = BackupModuleViewModel;
            this._backupHelper = backupHelper;

            SetNavigationCommand(Resources.MagentixBackup, Resources.Common, "Images/Market.png", 50);
            PermissionRegistry.RegisterPermission(PermissionNames.OpenBackup, PermissionCategories.Navigation, string.Format(Resources.CanNavigate_f, Resources.BackupRestore));
            EventServiceFactory.EventService.GetEvent<GenericEvent<WorkPeriod>>().Subscribe(new Action<EventParameters<WorkPeriod>>(this.OnWorkperiodStatusChanged));
        }

        private void OnWorkperiodStatusChanged(EventParameters<WorkPeriod> obj)
        {
            if (obj.Topic == "WorkPeriod Status Changed" && DatabaseToolsSettings.Settings.AutoCreateBackups && obj.Value.StartDate != obj.Value.EndDate)
            {
                this._backupHelper.CreateBackup(DatabaseToolsSettings.GetBackupLocation(), DatabaseToolsSettings.Settings.DatabaseName, 'A');
            }
        }

        protected override void OnInitialization()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(BackupModuleView));
        }

        protected override bool CanNavigate(string arg)
        {
            return _userService.IsUserPermittedFor(PermissionNames.OpenBackup);
        }

        protected override void OnNavigate(string obj)
        {
            base.OnNavigate(obj);
        }

        public override object GetVisibleView()
        {
            return _BackupModuleView;
        }
    }
}
