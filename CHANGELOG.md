# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2024-02-23

### Changed
- **BREAKING**: Switched from BepInEx to MelonLoader
  - Reason: Better support for Unity 2022.3+ (metadata v31)
  - BepInEx 6 doesn't support new Unity versions yet
  - MelonLoader is more stable for Il2Cpp games

### Added
- Initial MelonLoader release
- Configurable max player count (2-100 players)
- Default max players set to 20
- MelonLoader mod implementation
- Harmony patches for NetworkHandler
- Configuration file support via MelonPreferences

### Features
- Patches `NetworkHandler.Awake` to set max client count
- Patches `NetworkHandler.CreateLobbyAsync` to override lobby max players
- Patches `NetworkHandler.OnApprovingConnection` for connection logging
- Automatic config generation on first run
- Config validation (2-100 range)

### Technical Details
- Built with MelonLoader (latest)
- Uses Harmony for runtime patching
- Targets .NET 6.0
- Compatible with Il2Cpp games
- Supports Unity 2022.3.62f2 and newer

### Migration from BepInEx

If you were using the BepInEx version:

1. **Uninstall BepInEx**:
   - Delete `BepInEx/` folder
   - Delete `doorstop_config.ini`
   - Delete `winhttp.dll`

2. **Install MelonLoader**:
   - Download MelonLoader installer
   - Run and select game folder
   - Click "Install"

3. **Install mod**:
   - Copy `MorePlayers.dll` to `Mods/` folder
   - Run game to generate config
   - Configure in `UserData/MelonPreferences.cfg`

## [0.0.7] - 2024-02-23 (BepInEx - Deprecated)

### Note
This version and earlier used BepInEx and are no longer supported due to Unity version incompatibility.
