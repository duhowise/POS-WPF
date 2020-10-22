using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magentix.Infrastructure.Settings
{
    public class DatabaseToolsSettingsObject
    {
        public bool AllowRecovery
        {
            get;
            set;
        }

        public bool AllowReplace
        {
            get;
            set;
        }

        public bool AutoCreateBackups
        {
            get;
            set;
        }

        public bool AutoCreateBackupsOnImport
        {
            get;
            set;
        }

        public bool AutoCreateBackupsOnMigrate
        {
            get;
            set;
        }

        public string BackupLocation
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public string DatabaseName
        {
            get;
            set;
        }

        public bool ForceMigrateAfterRestoration
        {
            get;
            set;
        }

        public bool IsInTrainingMode
        {
            get;
            set;
        }

        public string LiveConnection
        {
            get;
            set;
        }

        public bool ManageLicenses
        {
            get;
            set;
        }

        public string TrainingDatabase
        {
            get;
            set;
        }

        public DatabaseToolsSettingsObject()
        {
            this.AutoCreateBackups = true;
            this.AutoCreateBackupsOnImport = true;
            this.AutoCreateBackupsOnMigrate = true;
            this.AllowReplace = true;
            this.AllowRecovery = false;
            this.ForceMigrateAfterRestoration = true;
            this.ManageLicenses = true;
        }
    }
}
