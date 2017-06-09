set projectName=Harry.Extensions.Logging.NLog

set version=1.0.0
set suffix=alpha1

set fullVersion=%version%-%suffix%

del /Q /S  ..\%projectName%\bin\ForNuget\*.*

dotnet restore
dotnet pack -c Release ../%projectName%  -o ../%projectName%/bin/ForNuget 
nuget.exe push ../%projectName%/bin/ForNuget/%projectName%.%fullVersion%.nupkg -Source https://www.nuget.org/api/v2/package


pause