@echo off
echo ========================================
echo Building MorePlayers Mod
echo ========================================
echo.

REM Проверка наличия dotnet
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET 6.0 SDK from https://dotnet.microsoft.com/download/dotnet/6.0
    pause
    exit /b 1
)

echo Restoring NuGet packages...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to restore packages!
    pause
    exit /b 1
)

echo.
echo Building in Release mode...
dotnet build -c Release
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Output: bin\Release\net6.0\MorePlayers.dll
echo.
echo To install:
echo 1. Install MelonLoader to your game folder
echo 2. Copy MorePlayers.dll to ^<Game Folder^>\Mods\
echo 3. Run the game
echo.
pause
