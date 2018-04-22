# WinServerChecker
A simple health checking tool for windows. Will check configured items and returns a response with the status of those checks. Great for uptime monitors like Pingdom.

This tool uses ASP.NET and the checks are configured through the web.config file.

Host in IIS with just a web.config and 1 DLL, or add to an existing website.

```
  <configSections>
    <section name="winServerChecker" type="WinServerChecker.Configuration.WinServerCheckerConfigurationSection,WinServerChecker" />
  </configSections>

  <winServerChecker authenticatorsOperator="ANY">
    <authenticators>
      <add name="HeaderAuth" type="WinServerChecker.Authentication.RequestHeaderAuthenticator,WinServerChecker" headerName="X-WinServerChecker" headerValue="12345" />
      <add name="UserAgentAuth" type="WinServerChecker.Authentication.UserAgentAuthenticator,WinServerChecker" contains="bullcrap" />
      <add name="SourceIpAuth" type="WinServerChecker.Authentication.SourceIpAuthenticator,WinServerChecker" ips="127.0.0.1;::1" />
    </authenticators>
    <checks>
      <add name="CheckForFile" type="WinServerChecker.Checks.FileExistsCheck,WinServerChecker" path="c:\somepath.txt" />
      <add name="UptimeCheck" type="WinServerChecker.Checks.UptimeCheck,WinServerChecker" />
      <add name="CDrive" type="WinServerChecker.Checks.DiskSpaceCheck,WinServerChecker" driveLetter="C" minPercentFreeSpace="10" />
      <add name="CPU" type="WinServerChecker.Checks.CPUCheck,WinServerChecker" />
      <add name="ComputerName" type="WinServerChecker.Checks.ComputerNameCheck,WinServerChecker" />
    </checks>
  </winServerChecker>
  <system.webServer>
    <handlers>
      <add name="WinServerChecker" verb="*" path="/" type="WinServerChecker.WinServerCheckerHandler,WinServerChecker"/>
    </handlers>
  </system.webServer>
  ```