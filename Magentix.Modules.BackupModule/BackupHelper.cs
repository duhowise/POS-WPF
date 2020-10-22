using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Magentix.Presentation.Services.Common;
using Magentix.Services;
using Microsoft.Practices.Prism.Events;

namespace Magentix.Modules.BackupModule
{
    [Export(typeof(BackupHelper))]
    public class BackupHelper
    {
        private readonly IBackupService _backupService;

        [ImportingConstructor]
        public BackupHelper(IBackupService backupService)
        {
            this._backupService = backupService;
        }

        public string CreateBackup(string backupLocation, string databaseName, char reason)
        {
            ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "Reset Cache", true);
            ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "CloseManagementTabs", true);
            return this._backupService.CreateDatabaseBackup(backupLocation, databaseName, reason);
        }

        public Task<string> CreateBackupAsync(string backupLocation, string databaseName, char reason)
        {
            ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "Reset Cache", true);
            ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "CloseManagementTabs", true);
            return _backupService.CreateDatabaseBackupAsync(backupLocation, databaseName, reason);
        }

        public void RestoreBackup(string databaseLabel, string databaseName, string backupFile, bool refreshCache = true)
        {
            if (refreshCache)
            {
                ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "Reset Cache", true);
            }
            ExtensionMethods.PublishEvent<EventAggregator>(EventServiceFactory.EventService, "CloseManagementTabs", true);
            this._backupService.RestoreDatabaseBackup(databaseLabel, databaseName, backupFile, false);
        }

        public void OverwriteBackup(string databaseLabel, string backupFile)
        {
            this._backupService.RestoreDatabaseBackup(databaseLabel, "", backupFile, true);
        }
    }
}
