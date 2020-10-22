using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;
using Magentix.Persistance.Data;
using Magentix.Presentation.Common;
using Magentix.Presentation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Magentix.Modules.BackupModule
{
    [Export]
    public class DatabaseTaskManager
    {
        private const string DefaultClearDatabaseScriptName = "[CBL]Clear Database Transactions.sql";

        private const string DefaultClearDatabaseScript = "DELETE FROM [TicketEntities]\r\nGO\r\nDELETE FROM [Tickets]\r\nGO\r\nDELETE FROM [AccountTransactionDocuments]\r\nGO\r\nDELETE FROM [AccountTransactions]\r\nGO\r\nDELETE FROM [AccountTransactionValues]\r\nGO\r\nDELETE FROM [Calculations]\r\nGO\r\nDELETE FROM [CostItems]\r\nGO\r\nDELETE FROM [InventoryTransactionDocuments]\r\nGO\r\nDELETE FROM [InventoryTransactions]\r\nGO\r\nDELETE FROM [Orders]\r\nGO\r\nDELETE FROM [PaidItems]\r\nGO\r\nDELETE FROM [PeriodicConsumptionItems]\r\nGO\r\nDELETE FROM [PeriodicConsumptions]\r\nGO\r\nDELETE FROM [ProductTimerValues]\r\nGO\r\nDELETE FROM [Payments]\r\nGO\r\nDELETE FROM [WarehouseConsumptions]\r\nGO\r\nDELETE FROM [WorkPeriods]\r\nGO\r\nUPDATE [Numerators] SET Number = 0\r\nGO\r\nUPDATE [EntityStateValues] SET EntityStates = '[{\"S\":\"Available\",\"SN\":\"Status\"}]' Where EntityStates like '%Status%'\r\nGO";

        private readonly IUserService _userService;

        private readonly ICommandExecutionService _executionService;

        private readonly IApplicationState _applicationState;

        private readonly BackupHelper _backupHelper;

        private static string TaskPath
        {
            get
            {
                return string.Concat(LocalSettings.DocumentPath, "\\Database Tasks");
            }
        }

        [ImportingConstructor]
        public DatabaseTaskManager(IUserService userService,  IApplicationState applicationState, ICommandExecutionService executionService, BackupHelper backupHelper)
        {
            this._userService = userService;
            this._executionService = executionService;
            this._applicationState = applicationState;
            this._backupHelper = backupHelper;
        }

        private void BackupIfRequested(string commands)
        {
            if (commands.Contains("B"))
            {
                string str = this._backupHelper.CreateBackup(DatabaseToolsSettings.GetBackupLocation(), DatabaseToolsSettings.Settings.DatabaseName, 'T');
                if (string.IsNullOrEmpty(str))
                {
                    MessageBox.Show("Backup Created Successfully", "Backup");
                    return;
                }
                MessageBox.Show(str, "Backup Error");
            }
        }

        public static bool CanCreateDefaultScripts()
        {
            return !Directory.Exists(DatabaseTaskManager.TaskPath);
        }

        private static bool ConfirmIfRequested(string commands, string commandName)
        {
            if (!commands.Contains("C"))
            {
                return true;
            }
            return MessageBox.Show(string.Format("Execute {0} Task?", commandName), Resources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private static void CreateDefaultClearDatabaseScript()
        {
            string str = Path.Combine(DatabaseTaskManager.TaskPath, "[CBL]Clear Database Transactions.sql");
            if (File.Exists(str))
            {
                File.Delete(str);
            }
            File.AppendAllText(str, "DELETE FROM [TicketEntities]\r\nGO\r\nDELETE FROM [Tickets]\r\nGO\r\nDELETE FROM [AccountTransactionDocuments]\r\nGO\r\nDELETE FROM [AccountTransactions]\r\nGO\r\nDELETE FROM [AccountTransactionValues]\r\nGO\r\nDELETE FROM [Calculations]\r\nGO\r\nDELETE FROM [CostItems]\r\nGO\r\nDELETE FROM [InventoryTransactionDocuments]\r\nGO\r\nDELETE FROM [InventoryTransactions]\r\nGO\r\nDELETE FROM [Orders]\r\nGO\r\nDELETE FROM [PaidItems]\r\nGO\r\nDELETE FROM [PeriodicConsumptionItems]\r\nGO\r\nDELETE FROM [PeriodicConsumptions]\r\nGO\r\nDELETE FROM [ProductTimerValues]\r\nGO\r\nDELETE FROM [Payments]\r\nGO\r\nDELETE FROM [WarehouseConsumptions]\r\nGO\r\nDELETE FROM [WorkPeriods]\r\nGO\r\nUPDATE [Numerators] SET Number = 0\r\nGO\r\nUPDATE [EntityStateValues] SET EntityStates = '[{\"S\":\"Available\",\"SN\":\"Status\"}]' Where EntityStates like '%Status%'\r\nGO");
        }

        public static void CreateDefaultScripts()
        {
            if (!Directory.Exists(DatabaseTaskManager.TaskPath))
            {
                Directory.CreateDirectory(DatabaseTaskManager.TaskPath);
            }
            DatabaseTaskManager.CreateDefaultClearDatabaseScript();
        }

        private void ExecuteAutomationCommand(string filePath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            if (!this._executionService.ConfirmAutomationCommand(fileNameWithoutExtension))
            {
                return;
            }
            this._applicationState.NotifyEvent("AutomationCommandExecuted", null);
        }

        public void ExecuteTask(DatabaseTask task, bool skipConfirmation = false)
        {
            if (!skipConfirmation && !DatabaseTaskManager.ConfirmIfRequested(task.Commands, task.NameStr))
            {
                return;
            }
            this.BackupIfRequested(task.Commands);
            this.ExecuteTask(task.TaskType, task.FilePath, task.Arguments);
            this.LogoutIfRequested(task.Commands);
        }

        private void ExecuteTask(string taskType, string filePath, string arguments)
        {
            filePath = DatabaseTaskManager.FixDirectoryName(filePath);
            if (taskType == "SQL")
            {
                Executor.ExecSqlCommand(filePath, new object[0]);
            }
            if (taskType == "BAT")
            {
                Executor.StartProcess(filePath, arguments);
            }
            if (taskType == "VBS")
            {
                Executor.StartVbsProcess(filePath, arguments);
            }
            if (taskType == "AC")
            {
                this.ExecuteAutomationCommand(filePath);
            }
        }

        private static string FixDirectoryName(string filePath)
        {
            if (!string.IsNullOrEmpty(Path.GetDirectoryName(filePath)))
            {
                return filePath;
            }
            return Path.Combine(DatabaseTaskManager.TaskPath, filePath);
        }

        private void LogoutIfRequested(string commands)
        {
            if (commands.Contains("L"))
            {
                this._userService.LogoutUser(true);
            }
        }

        public static IEnumerable<DatabaseTask> ReadDatabaseTasks()
        {
            string taskPath = DatabaseTaskManager.TaskPath;
            if (!Directory.Exists(taskPath))
            {
                return Enumerable.Empty<DatabaseTask>();
            }
            return
                from x in Directory.GetFiles(taskPath)
                select new DatabaseTask(x, "");
        }
    }
}
