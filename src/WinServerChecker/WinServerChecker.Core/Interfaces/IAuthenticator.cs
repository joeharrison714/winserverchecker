using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Core.Interfaces
{
    public interface IAuthenticator
    {
        void Initialize(IConfigurationSection parameters);
        bool Authenticate(HttpRequest request);
    }
}
