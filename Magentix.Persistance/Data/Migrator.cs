using Magentix.Infrastructure.Data.SQL;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Magentix.Infrastructure;
using Magentix.Infrastructure.Settings;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.IO;

namespace Magentix.Persistance.Data
{
    internal static class Migrator
    {
        internal static void DoMigrate(CommonDbContext context)
        {
            Migrator.DoMigrate(context.Database.Connection.ConnectionString);
        }

        internal static void DoMigrate(string connectionString)
        {
            ActionServices.Execute(ActionServiceType.PreMigration);
            string str = (connectionString.Contains(".sdf") ? "sqlserverce" : "sqlserver");
            RunnerContext runnerContext = new RunnerContext(new TextWriterAnnouncer(Console.Out))
            {
                ApplicationContext = str,
                Connection = connectionString,
                Database = str,
                Target = string.Concat(LocalSettings.AppPath, "\\Magentix.Persistance.DbMigration.dll")
            };
            (new TaskExecutor(runnerContext)).Execute();
            File.Delete(string.Concat(LocalSettings.UserPath, "\\migrate.txt"));
            ActionServices.Execute(ActionServiceType.PostMigration);
        }
    }
}
