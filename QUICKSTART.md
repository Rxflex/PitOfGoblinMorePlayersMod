# Quick Start Guide

## For Users

### Installation

1. **Install BepInEx**
   - Download BepInEx 5 IL2CPP from: https://github.com/BepInEx/BepInEx/releases
   - Extract to your game folder (where `Pit of Goblin.exe` is located)
   - Run the game once to generate BepInEx folders

2. **Install MorePlayers Mod**
   - Download `MorePlayers.dll` from releases
   - Copy to `BepInEx/plugins/` folder
   - Run the game

3. **Configure**
   - After first run, edit `BepInEx/config/com.rxflex.moreplayers.cfg`
   - Change `MaxPlayers` value (2-100)
   - Restart the game

### Configuration Example

```ini
[General]
MaxPlayers = 20
```

Change `20` to any number between 2 and 100.

## For Developers

### Building from Source

1. **Prerequisites**
   - .NET SDK 6.0 or later
   - Git

2. **Clone and Build**
   ```bash
   git clone <repository-url>
   cd MorePlayers
   dotnet restore
   dotnet build -c Release
   ```

3. **Output**
   - DLL will be in `bin/Release/netstandard2.1/MorePlayers.dll`

### Project Structure

```
MorePlayers/
├── MorePlayersPlugin.cs      # Main plugin class
├── PluginInfo.cs              # Plugin metadata
├── Patches/
│   └── NetworkHandlerPatches.cs  # Harmony patches
├── dump/                      # Game DLLs (not in repo)
└── MorePlayers.csproj         # Project file
```

### Testing

1. Copy `MorePlayers.dll` to `BepInEx/plugins/`
2. Run the game
3. Check `BepInEx/LogOutput.log` for mod messages
4. Try creating a lobby with more than 4 players

### Troubleshooting

**Mod doesn't load:**
- Check BepInEx is installed correctly
- Look for errors in `BepInEx/LogOutput.log`

**Game crashes:**
- Check if game was updated (may need new dump)
- Verify BepInEx version is compatible

**Max players not working:**
- Check config file was generated
- Verify patches applied (check logs)
- Make sure all players have the mod installed
