using Magentix.Modules.BackupModule;
using Magentix.Infrastructure.Settings;
using Magentix.Presentation.Services;
using Magentix.Services.Common;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Magentix.Modules.BackupModule.Actions
{
    [Export(typeof(IActionType))]
    internal class BackupDatabase : ActionType
    {
        private readonly IApplicationState _applicationState;

        private readonly IMethodQueue _methodQueue;

        private readonly BackupHelper _backupHelper;

        [ImportingConstructor]
        public BackupDatabase(IApplicationState applicationState, IMethodQueue methodQueue, BackupHelper backupHelper)
        {
            this._applicationState = applicationState;
            this._methodQueue = methodQueue;
            this._backupHelper = backupHelper;
        }

        protected override string GetActionKey()
        {
            return "BackupDatabase";
        }

        protected override string GetActionName()
        {
            return "Backup Database";
        }

        protected override object GetDefaultData()
        {
            return new { RunInBackground = true };
        }

        public override async void Process(ActionData actionData)
        {
            string str;
            if (!this._applicationState.IsLocked)
            {
                bool asBoolean = actionData.GetAsBoolean("RunInBackground", true);
                string backupLocation = DatabaseToolsSettings.GetBackupLocation();
                string databaseName = DatabaseToolsSettings.Settings.DatabaseName;
                if (LocalSettings.DatabaseLabel != "SQ" && LocalSettings.DatabaseLabel != "LD")
                {
                    asBoolean = false;
                }
                if (asBoolean)
                {
                    str = await this._backupHelper.CreateBackupAsync(backupLocation, databaseName, 'R');
                }
                else
                {
                    str = this._backupHelper.CreateBackup(backupLocation, databaseName, 'R');
                }
                string str1 = str;
                if (!string.IsNullOrEmpty(str1))
                {
                    MessageBox.Show(str1, "Database Backup Module", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                IMethodQueue methodQueue = this._methodQueue;
                methodQueue.Queue("BackupDatabase", () => this.Process(actionData));
            }
        }
    }
}
