using System;
using System.Threading.Tasks;

namespace Magentix.Services
{
    public interface IBackupService
    {
        string CreateDatabaseBackup(string backupLocation, string databaseName, char reason);

        Task<string> CreateDatabaseBackupAsync(string backupLocation, string databaseName, char reason);

        void RestoreDatabaseBackup(string databaseLabel, string databaseName, string backupFile, bool overwrite = false);
    }
}
