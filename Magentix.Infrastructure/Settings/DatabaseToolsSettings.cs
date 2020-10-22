using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Magentix.Infrastructure.Helpers;

namespace Magentix.Infrastructure.Settings
{
    public static class DatabaseToolsSettings
    {
        public static DatabaseToolsSettingsObject Settings
        {
            get;
            set;
        }

        static DatabaseToolsSettings()
        {
            DatabaseToolsSettings.LoadSettings();
        }

        public static string GetBackupLocation()
        {
            string backupLocation = DatabaseToolsSettings.Settings.BackupLocation;
            if (string.IsNullOrEmpty(backupLocation))
            {
                backupLocation = string.Concat(LocalSettings.DocumentPath, "\\Database Backups");
            }
            return backupLocation;
        }

        private static string GetSettingsData()
        {
            string settingsFileName = DatabaseToolsSettings.GetSettingsFileName();
            if (!SambaFile.Exists(settingsFileName))
            {
                return "";
            }
            return SambaFile.ReadAllText(settingsFileName);
        }

        private static string GetSettingsFileName()
        {
            return Path.Combine(DatabaseToolsSettings.GetSettingsPath(), "DatabaseToolsSettings.txt");
        }

        private static string GetSettingsPath()
        {
            return Path.GetDirectoryName(LocalSettings.SettingsFileName);
        }

        public static void LoadSettings()
        {
            DatabaseToolsSettings.Settings = JsonHelper.Deserialize<DatabaseToolsSettingsObject>(DatabaseToolsSettings.GetSettingsData());
        }

        public static void SaveSettings()
        {
            SambaFile.WriteAllText(DatabaseToolsSettings.GetSettingsFileName(), JsonHelper.Serialize<DatabaseToolsSettingsObject>(DatabaseToolsSettings.Settings));
        }
    }
}
