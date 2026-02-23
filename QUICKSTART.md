# Quick Start Guide

## For Users

### Installation

1. **Install MelonLoader**
   - Download from: https://github.com/LavaGang/MelonLoader/releases/latest
   - Run `MelonLoader.Installer.exe`
   - Select your game folder (where `Pit of Goblin.exe` is)
   - Click "Install"
   - Run the game once (it will take longer first time)
   - Close the game

2. **Install MorePlayers Mod**
   - Download `MorePlayers.dll` from releases
   - Copy to `<Game Folder>/Mods/` folder
   - Run the game

3. **Configure**
   - After first run, edit `<Game Folder>/UserData/MelonPreferences.cfg`
   - Find `[MorePlayers]` section
   - Change `MaxPlayers` value (2-100)
   - Save and restart the game

### Configuration Example

```ini
[MorePlayers]
MaxPlayers = 20
```

Change `20` to any number between 2 and 100.

## For Developers

### Building from Source

1. **Prerequisites**
   - .NET SDK 6.0 or later
   - Git
   - Game DLLs (see dump/README.md)

2. **Clone and Build**
   ```bash
   git clone <repository-url>
   cd MorePlayers
   dotnet restore
   dotnet build -c Release
   ```

3. **Output**
   - DLL will be in `bin/Release/net6.0/MorePlayers.dll`

### Project Structure

```
MorePlayers/
├── MorePlayersMod.cs              # Main mod class
├── Properties/AssemblyInfo.cs     # MelonLoader attributes
├── dump/                          # Game DLLs (not in repo)
└── MorePlayers.csproj             # Project file
```

### Testing

1. Copy `MorePlayers.dll` to `<Game Folder>/Mods/`
2. Run the game
3. Check `<Game Folder>/MelonLoader/Latest.log` for mod messages
4. Try creating a lobby with more than 4 players

### Troubleshooting

**Mod doesn't load:**
- Check MelonLoader is installed correctly
- Look for errors in `MelonLoader/Latest.log`
- Verify DLL is in `Mods/` folder

**Game crashes:**
- Check if game was updated (may need new dump)
- Verify MelonLoader version is compatible
- Look for stack traces in logs

**Max players not working:**
- Check config file was generated
- Verify patches applied (check logs)
- Make sure all players have the mod installed

## MelonLoader vs BepInEx

This mod uses **MelonLoader** because:
- Better support for new Unity versions (2022.3+)
- Easier installation
- More stable for Il2Cpp games
- Better logging and debugging

## Important Notes

- The mod must be installed on the **host** (lobby creator)
- All players should have the mod for best compatibility
- Config changes require game restart
- Check logs if something doesn't work
