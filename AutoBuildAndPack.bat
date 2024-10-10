@echo off
set CONFIGURATION=Release

echo Building and packing projects...
msbuild WinUICommunity.sln /t:Rebuild /p:Configuration=%CONFIGURATION% 
msbuild WinUICommunity.sln /t:Pack /p:Configuration=%CONFIGURATION% 

echo Done.
pause
