# WinServerChecker
A simple health checking tool for windows. Will check configured items and returns a response with the status of those checks. Great for uptime monitors like Pingdom.

This tool uses ASP.NET and the checks are configured through the web.config file.

Host in IIS with just a web.config and 1 DLL, or add to an existing website.