using Magentix.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Magentix.Modules.BackupModule
{
    public class BackupItem : ObservableObject
    {
        private string _name;

        private string _filePathPath;

        private DateTime _date;

        private string _databaseType;

        private string _backupReason;

        public string BackupReason
        {
            get
            {
                return this._backupReason;
            }
            set
            {
                this._backupReason = value;
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_BackupReason").MethodHandle)), new ParameterExpression[0]));
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_BackupReasonStr").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public string BackupReasonStr
        {
            get
            {
                string backupReason = this.BackupReason;
                string str = backupReason;
                if (backupReason != null)
                {
                    switch (str)
                    {
                        case "M":
                            {
                                return "Manual Backup";
                            }
                        case "A":
                            {
                                return "Auto Backup";
                            }
                        case "R":
                            {
                                return "Backup with Rule";
                            }
                        case "C":
                            {
                                return "Clear Transactions";
                            }
                        case "T":
                            {
                                return "Task Backup";
                            }
                        case "I":
                            {
                                return "Import Backup";
                            }
                        case "X":
                            {
                                return "Training Backup";
                            }
                        case "G":
                            {
                                return "Migrate Backup";
                            }
                    }
                }
                return "Unknown Reason";
            }
        }

        public string DatabaseType
        {
            get
            {
                return this._databaseType;
            }
            set
            {
                this._databaseType = value;
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_DatabaseType").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
                base.RaisePropertyChanged<DateTime>(Expression.Lambda<Func<DateTime>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_Date").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePathPath;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_Name").MethodHandle)), new ParameterExpression[0]));
                base.RaisePropertyChanged<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BackupItem)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BackupItem).GetMethod("get_NameStr").MethodHandle)), new ParameterExpression[0]));
            }
        }

        public string NameStr
        {
            get
            {
                if (string.IsNullOrEmpty(this.Name) || !this.Name.Contains("_"))
                {
                    return this.Name;
                }
                return this.Name.Substring(0, this.Name.IndexOf('\u005F'));
            }
        }

        public BackupItem(string filePath)
        {
            this.Name = Path.GetFileName(filePath);
            this.CreateBackupItem(filePath);
        }

        private void CreateBackupItem(string filePath)
        {
            this._filePathPath = filePath;
            if (File.Exists(filePath))
            {
                this.Name = Path.GetFileNameWithoutExtension(filePath);
            }
            this.Date = this.ParseDate(this.Name);
            this.DatabaseType = this.ParseDatabaseType(this.Name);
            this.BackupReason = this.ParseBackupReason(this.Name);
        }

        private string ParseBackupReason(string name)
        {
            string str;
            try
            {
                str = name.Substring(name.LastIndexOf("_", StringComparison.Ordinal) + 15, 1);
            }
            catch (Exception)
            {
                str = "";
            }
            return str;
        }

        private string ParseDatabaseType(string name)
        {
            string str;
            try
            {
                str = name.Substring(name.LastIndexOf("_", StringComparison.Ordinal) + 13, 2);
            }
            catch (Exception)
            {
                str = "";
            }
            return str;
        }

        private DateTime ParseDate(string name)
        {
            DateTime dateTime;
            try
            {
                string str = name.Substring(name.LastIndexOf("_", StringComparison.Ordinal) + 1, 12);
                int num = Convert.ToInt32(str.Substring(0, 4));
                int num1 = Convert.ToInt32(str.Substring(4, 2));
                int num2 = Convert.ToInt32(str.Substring(6, 2));
                int num3 = Convert.ToInt32(str.Substring(8, 2));
                int num4 = Convert.ToInt32(str.Substring(10, 2));
                dateTime = new DateTime(num, num1, num2, num3, num4, 0);
            }
            catch (Exception)
            {
                dateTime = DateTime.MinValue;
            }
            return dateTime;
        }
    }
}
