using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Core.Model
{
    public class WinServerCheckerResponse
    {
        public WinServerCheckerResponse()
        {
            Checks = new List<StatusCheckResponse>();
        }

        public DateTime Date { get; set; }
        public List<StatusCheckResponse> Checks { get; set; }

        public bool OverallPassed
        {
            get
            {
                bool overall = true;
                foreach (var scr in Checks)
                {
                    if (!scr.Passed) overall = false;
                }
                return overall;
            }
        }
    }
}
