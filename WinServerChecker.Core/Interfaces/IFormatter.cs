using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Interfaces
{
    public interface IFormatter
    {
        string ContentType
        {
            get;
        }

        string Format(WinServerCheckerResponse response);
    }
}
