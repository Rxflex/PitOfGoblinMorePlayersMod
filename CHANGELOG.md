# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2024-02-23

### Added
- Initial release
- Configurable max player count (2-100 players)
- Default max players set to 20
- BepInEx plugin implementation
- Harmony patches for NetworkHandler
- Configuration file support

### Features
- Patches `NetworkHandler.Awake` to set max client count
- Patches `NetworkHandler.CreateLobbyAsync` to override lobby max players
- Patches `NetworkHandler.OnApprovingConnection` for connection logging
- Automatic config generation on first run
- Config validation (2-100 range)

### Technical Details
- Built with BepInEx 5
- Uses Harmony for runtime patching
- Targets .NET Standard 2.1
- Compatible with Il2Cpp games
