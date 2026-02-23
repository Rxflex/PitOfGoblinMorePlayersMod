# Migration Guide: BepInEx → MelonLoader

## Why the Change?

The mod was rewritten from BepInEx to MelonLoader because:

- **Unity 2022.3+ Support**: Pit of Goblin uses Unity 2022.3.62f2 with metadata version 31
- **BepInEx Limitation**: BepInEx 6.0.0 only supports metadata versions 23-29
- **MelonLoader Advantage**: Better support for new Unity versions and Il2Cpp games

## For Users

### If You Had BepInEx Version

1. **Uninstall BepInEx**:
   ```
   Delete from game folder:
   - BepInEx/
   - doorstop_config.ini
   - winhttp.dll
   - .doorstop_version
   ```

2. **Install MelonLoader**:
   - Download from: https://github.com/LavaGang/MelonLoader/releases/latest
   - Run `MelonLoader.Installer.exe`
   - Select game folder
   - Click "Install"

3. **Install New Mod Version**:
   - Download `MorePlayers.dll` from releases
   - Copy to `<Game Folder>/Mods/` folder
   - Run game

4. **Migrate Config**:
   
   Old location (BepInEx):
   ```
   BepInEx/config/com.rxflex.moreplayers.cfg
   ```
   
   New location (MelonLoader):
   ```
   UserData/MelonPreferences.cfg
   ```
   
   Copy your `MaxPlayers` value to the new config:
   ```ini
   [MorePlayers]
   MaxPlayers = 20
   ```

### Fresh Installation

Just follow [INSTALL.md](INSTALL.md)

## For Developers

### Code Changes

**BepInEx version:**
```csharp
[BepInPlugin(GUID, NAME, VERSION)]
public class MorePlayersPlugin : BaseUnityPlugin
{
    private void Awake() { }
}
```

**MelonLoader version:**
```csharp
[assembly: MelonInfo(typeof(MorePlayersMod), NAME, VERSION, AUTHOR)]
public class MorePlayersMod : MelonMod
{
    public override void OnInitializeMelon() { }
}
```

### Project Changes

**Old (.csproj):**
- Target: `netstandard2.1`
- Dependencies: BepInEx NuGet packages
- Config: `NuGet.Config` for BepInEx feed

**New (.csproj):**
- Target: `net6.0`
- Dependencies: Direct DLL references
- No NuGet packages needed

### Build Changes

**Old:**
```bash
dotnet build -c Release
# Output: bin/Release/netstandard2.1/MorePlayers.dll
```

**New:**
```bash
dotnet build -c Release
# Output: bin/Release/net6.0/MorePlayers.dll
```

### Required DLLs

**BepInEx needed:**
- BepInEx.Core.dll (from NuGet)
- 0Harmony.dll (from NuGet)

**MelonLoader needs:**
- MelonLoader.dll (from game installation)
- 0Harmony.dll (from game installation)

Both in `dump/DummyDll/` folder.

## Configuration Comparison

### BepInEx Config
```ini
# File: BepInEx/config/com.rxflex.moreplayers.cfg
[General]
## Maximum number of players
# Setting type: Int32
# Default value: 20
# Acceptable value range: From 2 to 100
MaxPlayers = 20
```

### MelonLoader Config
```ini
# File: UserData/MelonPreferences.cfg
[MorePlayers]
MaxPlayers = 20
```

Both work the same way, just different locations.

## Compatibility

### What Stayed the Same
- ✅ Harmony patches (same logic)
- ✅ Configuration values (2-100 range)
- ✅ Default max players (20)
- ✅ Functionality (works identically)

### What Changed
- ❌ Framework (BepInEx → MelonLoader)
- ❌ Installation method
- ❌ Config file location
- ❌ Folder structure

## Troubleshooting

### "I can't find my old config"

BepInEx config was in:
```
BepInEx/config/com.rxflex.moreplayers.cfg
```

If you still have it, copy the `MaxPlayers` value to:
```
UserData/MelonPreferences.cfg
```

### "Mod doesn't load"

Make sure:
1. MelonLoader is installed (not BepInEx)
2. DLL is in `Mods/` folder (not `BepInEx/plugins/`)
3. Check `MelonLoader/Latest.log` for errors

### "Game crashes"

1. Verify MelonLoader installed correctly
2. Try running game without mod
3. Check logs for error messages
4. Report issue with log file

## Questions?

- Open an issue: https://github.com/Rxflex/MorePlayers/issues
- Check logs: `MelonLoader/Latest.log`
- Read docs: [README.md](README.md), [INSTALL.md](INSTALL.md)
