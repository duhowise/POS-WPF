using Magentix.Presentation.Common.ModelBase;
using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;
using Magentix.Presentation.Common.Commands;
using Magentix.Presentation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Magentix.Modules.BackupModule
{
    [Export(typeof(BackupModuleViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BackupModuleViewModel : VisibleViewModelBase
    {
        private readonly IUserService _userService;

        private readonly BackupHelper _backupHelper;

        private IEnumerable<BackupItem> _backups;

        public bool AllowReplace
        {
            get
            {
                return DatabaseToolsSettings.Settings.AllowReplace;
            }
            set
            {
                DatabaseToolsSettings.Settings.AllowReplace = value;
                base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_AllowReplace").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public bool AutoCreateBackups
        {
            get
            {
                return DatabaseToolsSettings.Settings.AutoCreateBackups;
            }
            set
            {
                DatabaseToolsSettings.Settings.AutoCreateBackups = value;
                base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_AutoCreateBackups").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public bool AutoCreateBackupsOnMigrate
        {
            get
            {
                return DatabaseToolsSettings.Settings.AutoCreateBackupsOnMigrate;
            }
            set
            {
                DatabaseToolsSettings.Settings.AutoCreateBackupsOnMigrate = (value);
                base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_AutoCreateBackupsOnMigrate").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public ICaptionCommand BackupDatabaseCommand
        {
            get;
            set;
        }

        public string BackupLocation
        {
            get
            {
                return DatabaseToolsSettings.Settings.BackupLocation;
            }
            set
            {
                DatabaseToolsSettings.Settings.BackupLocation = (value);
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_BackupLocation").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public IEnumerable<BackupItem> Backups
        {
            get
            {
                IEnumerable<BackupItem> backupItems = this._backups;
                if (backupItems == null)
                {
                    IEnumerable<BackupItem> backupItems1 = this.ReadBackups();
                    IEnumerable<BackupItem> backupItems2 = backupItems1;
                    this._backups = backupItems1;
                    backupItems = backupItems2;
                }
                return backupItems;
            }
        }

        public ICaptionCommand ClearBackupLocationCommand
        {
            get;
            set;
        }

        public string DatabaseName
        {
            get
            {
                return DatabaseToolsSettings.Settings.DatabaseName;
            }
            set
            {
                DatabaseToolsSettings.Settings.DatabaseName = (value);
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_DatabaseName").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public ICaptionCommand DeleteBackupCommand
        {
            get;
            set;
        }

        public bool ForceMigrateAfterRestoration
        {
            get
            {
                return DatabaseToolsSettings.Settings.ForceMigrateAfterRestoration;
            }
            set
            {
                DatabaseToolsSettings.Settings.ForceMigrateAfterRestoration = (value);
                base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_ForceMigrateAfterRestoration").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public ICaptionCommand OpenBackupLocationCommand
        {
            get;
            set;
        }

        public ICaptionCommand RestoreDatabaseCommand
        {
            get;
            set;
        }

        public ICaptionCommand RestoreFromFileCommand
        {
            get;
            set;
        }

        public ICaptionCommand SaveSettingsCommand
        {
            get;
            set;
        }

        public ICaptionCommand SelectBackupLocationCommand
        {
            get;
            set;
        }

        public BackupItem SelectedBackup
        {
            get;
            set;
        }

        // For Database Tools
        private readonly DatabaseTaskManager _databaseTaskManager;

        private IEnumerable<DatabaseTask> _databaseTasks;

        public ICaptionCommand CreateDefaultTasksCommand
        {
            get;
            set;
        }

        public IEnumerable<DatabaseTask> DatabaseTasks
        {
            get
            {
                IEnumerable<DatabaseTask> databaseTasks = this._databaseTasks;
                if (databaseTasks == null)
                {
                    IEnumerable<DatabaseTask> databaseTasks1 = DatabaseTaskManager.ReadDatabaseTasks();
                    IEnumerable<DatabaseTask> databaseTasks2 = databaseTasks1;
                    this._databaseTasks = databaseTasks1;
                    databaseTasks = databaseTasks2;
                }
                return databaseTasks;
            }
        }

        public ICaptionCommand DisableTrainingModeCommand
        {
            get;
            set;
        }

        public ICaptionCommand ExecuteDatabaseTaskCommand
        {
            get;
            set;
        }

        public bool IsCreateDefaultTasksButtonVisible
        {
            get
            {
                return DatabaseTaskManager.CanCreateDefaultScripts();
            }
        }

        public DatabaseTask SelectedDatabaseTask
        {
            get;
            set;
        }

        [ImportingConstructor]
        public BackupModuleViewModel(IUserService userService, BackupHelper backupHelper, DatabaseTaskManager databaseTaskManager)
        {
            this._userService = userService;
            this._backupHelper = backupHelper;
            this._databaseTaskManager = databaseTaskManager;
            this.SaveSettingsCommand = new CaptionCommand<string>(Resources.Save, new Action<string>(this.OnSaveSettings));
            this.SelectBackupLocationCommand = new CaptionCommand<string>("...", new Action<string>(this.OnSelectBackupLocation));
            this.ClearBackupLocationCommand = new CaptionCommand<string>("X", new Action<string>(this.OnClearBackupLocation));
            this.BackupDatabaseCommand = new CaptionCommand<string>(Resources.BackupDatabase, new Action<string>(this.OnBackupDatabase));
            this.RestoreDatabaseCommand = new CaptionCommand<string>(Resources.RestoreDatabase, new Action<string>(this.OnRestoreDatabase), new Func<string, bool>(this.CanRestoreDatabase));
            this.DeleteBackupCommand = new CaptionCommand<string>(Resources.DeleteBackup, new Action<string>(this.OnDeleteBackup), new Func<string, bool>(this.CanDeleteBackup));
            this.OpenBackupLocationCommand = new CaptionCommand<string>(Resources.DisplayBackupLocation, new Action<string>(this.OnDisplayBackupLocation));
            this.RestoreFromFileCommand = new CaptionCommand<string>(Resources.RestoreFromZip, new Action<string>(this.OnRestoreFromFile));


            this.ExecuteDatabaseTaskCommand = new CaptionCommand<string>(Resources.ExecuteTask, new Action<string>(this.OnExecuteDatabaseTask), new Func<string, bool>(this.CanExecuteDatabaseTask));
            this.CreateDefaultTasksCommand = new CaptionCommand<string>(Resources.CreateDefaultTasks, new Action<string>(this.OnCreateDefaultTasks), new Func<string, bool>(this.CanCreateDefaultTasks));
        }

        private bool CanDeleteBackup(string arg)
        {
            return this.SelectedBackup != null;
        }

        private bool CanRestoreDatabase(string arg)
        {
            if (this.SelectedBackup == null || string.IsNullOrEmpty(this.SelectedBackup.DatabaseType))
            {
                return false;
            }
            return this.SelectedBackup.Date != DateTime.MinValue;
        }

        protected override string GetHeaderInfo()
        {
            return "Database Backups";
        }

        public override Type GetViewType()
        {
            return typeof(BackupModuleView);
        }

        private void OnBackupDatabase(string obj)
        {
            if (DatabaseToolsSettings.Settings.IsInTrainingMode)
            {
                MessageBox.Show("Do not backup databases while in training mode.", "Backup");
                return;
            }
            string str = this._backupHelper.CreateBackup(DatabaseToolsSettings.GetBackupLocation(), DatabaseToolsSettings.Settings.DatabaseName, 'M');
            this.RefreshBackups();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("Backup Created Successfully", "Backup");
                return;
            }
            MessageBox.Show(str, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void OnClearBackupLocation(string obj)
        {
            this.BackupLocation = null;
        }

        private void OnDeleteBackup(string obj)
        {
            if (File.Exists(this.SelectedBackup.FilePath) && MessageBox.Show(string.Format("Delete Backup [{0}]?", this.SelectedBackup.Name), Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                File.Delete(this.SelectedBackup.FilePath);
                this._backups = null;
                base.RaisePropertyChanged<IEnumerable<BackupItem>>(Expression.Lambda<Func<IEnumerable<BackupItem>>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_Backups").MethodHandle)), new ParameterExpression[0]));
            }
        }

        private void OnDisplayBackupLocation(string obj)
        {
            Process process = new Process();
            process.StartInfo.FileName = DatabaseToolsSettings.GetBackupLocation();
            process.Start();
        }

        private void OnRestoreDatabase(string obj)
        {
            this.RestoreBackup(this.SelectedBackup.DatabaseType, DatabaseToolsSettings.Settings.DatabaseName, this.SelectedBackup.FilePath);
        }

        private void OnRestoreFromFile(string obj)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                InitialDirectory = DatabaseToolsSettings.GetBackupLocation(),
                DefaultExt = "*.zip",
                Filter = "ZIP Files (*.zip)|*.zip"
            };
            Microsoft.Win32.OpenFileDialog openFileDialog1 = openFileDialog;
            bool? nullable = openFileDialog1.ShowDialog();
            if ((!nullable.GetValueOrDefault() ? false : nullable.HasValue))
            {
                string fileName = openFileDialog1.FileName;
                this.RestoreBackup(LocalSettings.DatabaseLabel, DatabaseToolsSettings.Settings.DatabaseName, fileName);
            }
        }

        private void OnSaveSettings(string obj)
        {
            DatabaseToolsSettings.SaveSettings();
            MessageBox.Show("Settings Saved", "Backup");
        }

        private void OnSelectBackupLocation(string obj)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = this.BackupLocation
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackupLocation = folderBrowserDialog.SelectedPath;
            }
        }

        private IEnumerable<BackupItem> ReadBackups()
        {
            if (!Directory.Exists(DatabaseToolsSettings.GetBackupLocation()))
                Directory.CreateDirectory(DatabaseToolsSettings.GetBackupLocation());
            return (
                from x in Directory.GetFiles(DatabaseToolsSettings.GetBackupLocation(), "*.zip")
                select new BackupItem(x)).AsParallel<BackupItem>().OrderByDescending<BackupItem, DateTime>((BackupItem x) => x.Date);
        }

        private void RefreshBackups()
        {
            this._backups = null;
            base.RaisePropertyChanged<IEnumerable<BackupItem>>(Expression.Lambda<Func<IEnumerable<BackupItem>>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_Backups").MethodHandle)), new ParameterExpression[0]));
        }

        private void RestoreBackup(string databaseType, string databaseName, string filePath)
        {
            if (DatabaseToolsSettings.Settings.IsInTrainingMode)
            {
                MessageBox.Show("Do not restore databases while in training mode.", "Restoration");
                return;
            }
            if (MessageBox.Show("Be sure all terminals disconnected before restoring database.\r\rDo you want to continue?", "Restoration", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    this._backupHelper.RestoreBackup(databaseType, databaseName, filePath, true);
                    MessageBox.Show("Database Restored Successfully. Please run this software again now.", "Restoration");
                    System.Windows.Application.Current.Shutdown();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private bool CanCreateDefaultTasks(string arg)
        {
            return DatabaseTaskManager.CanCreateDefaultScripts();
        }

        private bool CanExecuteDatabaseTask(string arg)
        {
            return this.SelectedDatabaseTask != null;
        }

        private void OnCreateDefaultTasks(string obj)
        {
            DatabaseTaskManager.CreateDefaultScripts();
            this._databaseTasks = null;
            base.RaisePropertyChanged<IEnumerable<DatabaseTask>>(Expression.Lambda<Func<IEnumerable<DatabaseTask>>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_DatabaseTasks").MethodHandle)), new ParameterExpression[0]));
            base.RaisePropertyChanged<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BackupModuleViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupModuleViewModel).GetMethod("get_IsCreateDefaultTasksButtonVisible").MethodHandle)), new ParameterExpression[0]));
        }


        private void OnExecuteDatabaseTask(string obj)
        {
            this._databaseTaskManager.ExecuteTask(this.SelectedDatabaseTask, false);
        }
    }
}
