using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Interfaces;
using WinServerChecker.Model;

namespace WinServerChecker.Formatters
{
    public class JsonFormatter : IFormatter
    {
        public string ContentType { get { return "application/json"; } }

        public string Format(WinServerCheckerResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");

            sb.Append("\t");
            sb.AppendLine(string.Format("\"{0}\": \"{1}\",", "date", response.Date.ToString("o")));

            sb.Append("\t");
            sb.AppendLine(string.Format("\"{0}\":[", "checks"));

            int checkIndex = 0;
            foreach (var check in response.Checks)
            {
                if (checkIndex > 0) sb.AppendLine(",");

                sb.AppendLine("\t{");
                sb.Append(GetElements(check));
                sb.Append("\t}");

                checkIndex++;
            }

            sb.AppendLine("]");

            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GetElements(StatusCheckResponse check)
        {
            List<KeyValuePair<string, object>> allData = new List<KeyValuePair<string, object>>();
            allData.Add(new KeyValuePair<string, object>("name", check.Name));
            allData.Add(new KeyValuePair<string, object>("passed", check.Passed));

            if (!string.IsNullOrWhiteSpace(check.Message))
            {
                allData.Add(new KeyValuePair<string, object>("message", check.Message));
            }

            foreach (var extraDataItem in check.Data)
            {
                if (String.IsNullOrWhiteSpace(extraDataItem.Key)) continue;
                if (extraDataItem.Value == null) continue;

                allData.Add(new KeyValuePair<string, object>(extraDataItem.Key, extraDataItem.Value));
            }

            StringBuilder sb = new StringBuilder();
            foreach(var kvp in allData)
            {
                string val = "";
                val = GetVal(kvp.Value);

                if (sb.Length > 0) sb.AppendLine(",");
                sb.Append("\t\t");
                sb.Append(string.Format("\"{0}\": {1}", kvp.Key, val));
            }
            sb.AppendLine("");
            return sb.ToString();
        }

        private static string GetVal(object source)
        {
            string val = "";
            bool quoted = true;

            if (source is string)
            {
                quoted = true;
                val = (string)source;
            }
            else if (source is DateTime)
            {
                quoted = true;
                val = ((DateTime)source).ToString("o");
            }
            else if (source is bool ||
                source is int ||
                source is double ||
                source is decimal ||
                source is float ||
                source is long)
            {
                quoted = false;
                val = source.ToString();
            }
            else
            {
                quoted = true;
                val = source.ToString();
            }

            
            if (quoted)
            {
                return "\"" + Encode(val) + "\"";
            }
            else
            {
                return val;
            }
        }

        private static string Encode(string name)
        {
            string encoded = name.Replace("\"", "\\\"");
            return encoded;
        }
    }
}

//{
//	"date": "2016-01-27T19:02:20.5490058Z",
//	"checks":[
//	{
//		"name": "QuickbooksFileStatus",
//		"passed": true,
//		"message": null
//	},
//	{
//		"name": "DriveSpaceStatus",
//		"passed": true,
//		"message": null
//	}]
//}