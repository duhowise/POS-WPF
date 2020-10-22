using Magentix.Modules.BackupModule;
using Magentix.Infrastructure.Settings;
using Magentix.Services.Common;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Magentix.Modules.BackupModule.Actions
{
    [Export(typeof(IActionType))]
    internal class RestoreDatabase : ActionType
    {
        private readonly BackupHelper _backupHelper;

        [ImportingConstructor]
        public RestoreDatabase(BackupHelper backupHelper)
        {
            this._backupHelper = backupHelper;
        }

        protected override string GetActionKey()
        {
            return "RestoreDatabase";
        }

        protected override string GetActionName()
        {
            return "Restore Database";
        }

        protected override object GetDefaultData()
        {
            return new { FileToRestore = "" };
        }

        public override void Process(ActionData actionData)
        {
            string asString = actionData.GetAsString("FileToRestore");
            if (!string.IsNullOrEmpty(asString))
            {
                try
                {
                    BackupItem backupItem = new BackupItem(asString);
                    this._backupHelper.RestoreBackup(backupItem.DatabaseType, DatabaseToolsSettings.Settings.DatabaseName, backupItem.FilePath, true);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    MessageBox.Show(exception.Message, "Database Backup Module", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }
    }
}
