set projectName=Harry.Extensions.Logging.NLog

nuget.exe push ../%projectName%/bin/Release/%projectName%.1.0.0-alpha3.nupkg -Source https://www.nuget.org/api/v2/package


pause