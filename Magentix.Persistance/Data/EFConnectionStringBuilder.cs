using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Magentix.Persistance.Data
{
    public class EFConnectionStringBuilder
    {
        private readonly string _connectionString;

        private readonly Dictionary<string, string> _values;

        public string Database
        {
            get
            {
                return this.GetValue("database");
            }
            set
            {
                this.SetValue("database", value);
            }
        }

        public string DataSource
        {
            get
            {
                return this.GetValue("data source");
            }
            set
            {
                this.SetValue("data source", value);
            }
        }

        public string InitialCatalog
        {
            get
            {
                return this.GetValue("initial catalog");
            }
            set
            {
                this.SetValue("initial catalog", value);
            }
        }

        public string Password
        {
            get
            {
                return this.GetValue("password");
            }
            set
            {
                this.SetValue("password", value);
            }
        }

        public string UserId
        {
            get
            {
                return this.GetValue("user id");
            }
            set
            {
                this.SetValue("user id", value);
            }
        }

        private EFConnectionStringBuilder(string connectionString)
        {
            this._connectionString = connectionString;
            this._values = DictionaryParser.ParseConnectionString(this._connectionString);
        }

        public string Build()
        {
            string str = string.Concat(this.GetFValue("Data Source"), this.GetFValue("User Id"), this.GetFValue("Password"));
            return EFConnectionStringBuilder.Fix(str);
        }

        public string BuildRaw()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in
                from x in this._values
                where x.Key != "attachdbfilename"
                select x)
            {
                stringBuilder.Append(this.GetFValue(keyValuePair.Key));
            }
            return stringBuilder.ToString();
        }

        public string BuildSetting()
        {
            if (string.IsNullOrEmpty(this.DataSource) && !string.IsNullOrEmpty(this.Database))
            {
                return this.Database;
            }
            if (string.IsNullOrEmpty(this.DataSource))
            {
                return "";
            }
            if (this.DataSource.Contains(".mdf"))
            {
                return this.DataSource;
            }
            if (this.DataSource.Contains(".sdf") && !this.DataSource.Contains(":"))
            {
                return this.DataSource;
            }
            if (this.DataSource.Contains(".sdf"))
            {
                return this.GetFValue("Data Source");
            }
            if (this.DataSource.Contains(".txt"))
            {
                return this.DataSource;
            }
            string str = string.Concat(this.GetFValue("Data Source"), this.GetFValue("User Id"), this.GetFValue("Password"), this.GetFValue("Database"));
            return str;
        }

        public static EFConnectionStringBuilder Create(string connectionString)
        {
            return new EFConnectionStringBuilder(connectionString);
        }

        public static string Fix(string connectionString)
        {
            if (!connectionString.Trim().EndsWith(";"))
            {
                connectionString = string.Concat(connectionString, ";");
            }
            if (!connectionString.ToLower().Contains("multipleactiveresultsets"))
            {
                connectionString = string.Concat(connectionString, "MultipleActiveResultSets = True;");
            }
            if (!connectionString.ToLower(CultureInfo.InvariantCulture).Contains("user id") && !connectionString.ToLower(CultureInfo.InvariantCulture).Contains("integrated security"))
            {
                connectionString = string.Concat(connectionString, " Integrated Security = True;");
            }
            if (connectionString.ToLower(CultureInfo.InvariantCulture).Contains("user id") && !connectionString.ToLower().Contains("persist security info"))
            {
                connectionString = string.Concat(connectionString, " Persist Security Info = True;");
            }
            return connectionString;
        }

        public string Fix()
        {
            return EFConnectionStringBuilder.Fix(this._connectionString);
        }

        private string GetFValue(string key)
        {
            string str = key.ToLower(CultureInfo.InvariantCulture).Trim();
            if (!this._values.ContainsKey(str))
            {
                return "";
            }
            string item = this._values[str];
            if (string.IsNullOrEmpty(item))
            {
                return "";
            }
            return string.Concat(key, "=", item, "; ");
        }

        public string GetValue(string key)
        {
            if (!this._values.ContainsKey(key))
            {
                return null;
            }
            return this._values[key];
        }

        public void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "";
            }
            this._values[key] = value.Trim();
        }

        public EFConnectionStringBuilder WithDataSource(string dataSource)
        {
            this.DataSource = dataSource;
            return this;
        }

        public EFConnectionStringBuilder WithPassword(string password)
        {
            this.Password = password;
            return this;
        }

        public EFConnectionStringBuilder WithUserId(string userId)
        {
            this.UserId = userId;
            return this;
        }
    }
}
