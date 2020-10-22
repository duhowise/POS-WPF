using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Magentix.Modules.BackupModule
{
    public class DatabaseTask
    {
        public string Arguments
        {
            get;
            set;
        }

        public string Commands
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string NameStr
        {
            get
            {
                return this.Name.Replace(string.Format("[{0}]", this.Commands), "");
            }
        }

        public string TaskType
        {
            get;
            set;
        }

        public DatabaseTask(string filePath, string arguments = "")
        {
            this.Name = Path.GetFileNameWithoutExtension(filePath);
            string upper = (Path.GetExtension(filePath) ?? "").ToUpper();
            char[] chrArray = new char[] { '.' };
            this.TaskType = upper.Trim(chrArray);
            this.FilePath = filePath;
            this.Commands = this.ExtractCommands(this.Name);
            this.Arguments = arguments;
        }

        private string ExtractCommands(string name)
        {
            if (!Regex.IsMatch(name, "\\[([^\\)]+)\\]"))
            {
                return "";
            }
            return Regex.Match(name, "\\[([^\\)]+)\\]").Groups[1].Value;
        }
    }
}
