using Magentix.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Magentix.Persistance.Data
{
    public static class Executor
    {
        private static void Exec(SqlConnection sqlConnection, string commandText, object[] parameters)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = Executor.GetCommand(commandText, parameters);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void ExecSqlCommand(string commandText, params object[] parameters)
        {
            using (IWorkspace workspace = WorkspaceFactory.Create())
            {
                foreach (string str in Executor.FixCommandText(commandText))
                {
                    workspace.ExecSql(Executor.GetCommand(str, parameters));
                }
            }
        }

        public static void ExecSqlCommand(SqlConnection sqlConnection, string commandText, params object[] parameters)
        {
            foreach (string str in Executor.FixCommandText(commandText))
            {
                Executor.Exec(sqlConnection, str, parameters);
            }
        }

        private static IEnumerable<string> FixCommandText(string commandText)
        {
            string str = commandText;
            try
            {
                if (commandText.ToLower().EndsWith(".sql") && File.Exists(commandText))
                {
                    str = File.ReadAllText(commandText);
                }
            }
            catch
            {
                str = commandText;
            }
            string[] strArrays = new string[] { "GO" };
            return
                from x in str.Split(strArrays, StringSplitOptions.RemoveEmptyEntries)
                select x.Trim(new char[] { '\r', '\n' }) into x
                where !string.IsNullOrWhiteSpace(x)
                select x;
        }

        private static string GetCommand(string command, object[] parameters)
        {
            if (!parameters.Any<object>())
            {
                return command;
            }
            return string.Format(command, parameters);
        }

        public static SqlDataReader GetSqlReader(SqlConnection sqlConnection, string commandText, params object[] parameters)
        {
            SqlDataReader sqlDataReader;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = Executor.GetCommand(commandText, parameters);
                sqlDataReader = sqlCommand.ExecuteReader();
            }
            return sqlDataReader;
        }

        public static void StartProcess(string filePath, string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", string.Format("/c \"{0}\" {1}", filePath, arguments))
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
        }

        public static void StartVbsProcess(string filePath, string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cscript.exe")
            {
                Arguments = string.Format("//NoLogo \"{0}\" {1}", filePath, arguments),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
        }
    }
}
