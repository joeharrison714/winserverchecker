using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Formatters
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

                sb.Append("\t\t");
                sb.AppendLine(string.Format("\"{0}\": \"{1}\",", "name", check.Name));

                sb.Append("\t\t");
                sb.Append(string.Format("\"{0}\": {1}", "passed", check.Passed.ToString().ToLower()));

                if (!string.IsNullOrWhiteSpace(check.Message))
                {
                    sb.AppendLine(",");

                    string messageEscaped = check.Message.Replace("\"", "\\\"");

                    sb.Append("\t\t");
                    sb.AppendLine(string.Format("\"{0}\": \"{1}\"", "message", messageEscaped));
                }
                else
                {
                    sb.AppendLine("");
                }

                sb.Append("\t}");

                checkIndex++;
                //if ()
            }

            sb.AppendLine("]");

            sb.AppendLine("}");

            return sb.ToString();
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