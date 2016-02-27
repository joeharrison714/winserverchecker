using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Model;

namespace WinServerChecker.Interfaces
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
