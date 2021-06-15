# WinServerChecker
A simple health checking tool for windows. Will check configured items and returns a response with the status of those checks. Great for uptime monitors like Pingdom.

This tool uses ASP.NET and the checks are configured through the appsettings.json file.

## Build
from src dir:
`dotnet publish -c Release`

## Deploy
Copy files from `src\WinServerChecker\WinServerChecker.WebApi\bin\Release\net5.0\publish`

## Scheduled Task
