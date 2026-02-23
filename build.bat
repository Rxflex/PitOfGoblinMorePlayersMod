@echo off
echo Building MorePlayers mod...
dotnet restore
dotnet build -c Release
echo.
echo Build complete! DLL is in bin\Release\netstandard2.1\MorePlayers.dll
pause
