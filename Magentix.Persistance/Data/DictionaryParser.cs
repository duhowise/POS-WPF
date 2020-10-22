using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Magentix.Persistance.Data
{
    public class DictionaryParser
    {
        public DictionaryParser()
        {
        }

        public static string GetKey(string value)
        {
            if (!value.Contains("=") && value.ToLower().EndsWith(".sdf"))
            {
                return "database";
            }
            if (!value.Contains("=") && value.ToLower().EndsWith(".mdf"))
            {
                return "database";
            }
            char[] chrArray = new char[] { '=' };
            return value.Split(chrArray)[0].Trim().ToLower(CultureInfo.InvariantCulture);
        }

        public static string GetValue(string value)
        {
            if (!value.Contains("="))
            {
                if (!value.EndsWith(".sdf") && !value.EndsWith(".mdf"))
                {
                    return "";
                }
                return value;
            }
            char[] chrArray = new char[] { '=' };
            return value.Split(chrArray, 2)[1].Trim();
        }

        public static Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            char[] chrArray = new char[] { ';' };
            return connectionString.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).ToDictionary<string, string, string>(new Func<string, string>(DictionaryParser.GetKey), new Func<string, string>(DictionaryParser.GetValue));
        }
    }
}
