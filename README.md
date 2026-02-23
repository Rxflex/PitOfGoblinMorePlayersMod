# MorePlayers Mod for Pit of Goblin

[![Build and Release](https://github.com/Rxflex/MorePlayers/actions/workflows/build.yml/badge.svg)](https://github.com/Rxflex/MorePlayers/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Increases the maximum player count in Pit of Goblin from 4 to a configurable amount (default: 20).

## ✨ Features

- 🎮 Configurable maximum player count (2-100)
- ⚙️ Easy to install and configure
- 🔧 Works with MelonLoader
- 📝 Automatic config generation
- 🛡️ Safe and tested

## 📦 Installation

### For Users

1. **Install MelonLoader**
   - Download [MelonLoader](https://github.com/LavaGang/MelonLoader/releases/latest)
   - Download `MelonLoader.Installer.exe`
   - Run the installer and select your game folder
   - Click "Install"
   - Run the game once to generate folders

2. **Install MorePlayers Mod**
   - Download `MorePlayers.dll` from [Releases](https://github.com/Rxflex/MorePlayers/releases)
   - Copy to `<Game Folder>/Mods/` folder
   - Run the game

3. **Configure (Optional)**
   - After first run, edit `<Game Folder>/UserData/MelonPreferences.cfg`
   - Find `[MorePlayers]` section
   - Change `MaxPlayers` value (2-100)
   - Save and restart the game

## ⚙️ Configuration

The config file is located at `<Game Folder>/UserData/MelonPreferences.cfg`

```ini
[MorePlayers]
MaxPlayers = 20
```

Change `20` to any number between 2 and 100.

## 🔨 Building from Source

### Prerequisites
- [.NET SDK 6.0](https://dotnet.microsoft.com/download) or later
- Git
- Game DLLs (see `dump/README.md`)

### Build Steps

```bash
# Clone the repository
git clone https://github.com/Rxflex/MorePlayers.git
cd MorePlayers

# Restore dependencies
dotnet restore

# Build
dotnet build -c Release
```

The compiled DLL will be in `bin/Release/net6.0/MorePlayers.dll`

Or simply run `build.bat` on Windows.

## 📋 Requirements

- **MelonLoader** (latest version)
- **Pit of Goblin** (Il2Cpp version)
- **.NET 6.0** runtime (included with MelonLoader)

## 🐛 Troubleshooting

### Mod doesn't load
- Verify MelonLoader is installed correctly
- Check `<Game Folder>/MelonLoader/Latest.log` for errors
- Make sure DLL is in `Mods/` folder, not `Plugins/`

### Game crashes
- Check if game was updated (may need new mod version)
- Verify MelonLoader version is compatible
- Look for error messages in `MelonLoader/Latest.log`

### Max players not working
- Ensure config file was generated in `UserData/`
- Verify patches applied successfully (check logs)
- Make sure the host has the mod installed
- All players should have the mod for best compatibility

## 🤝 Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- MelonLoader team for the modding framework
- Harmony for runtime patching
- Pit of Goblin developers

## 📞 Support

- 🐛 [Report a bug](https://github.com/Rxflex/MorePlayers/issues/new?template=bug_report.md)
- 💡 [Request a feature](https://github.com/Rxflex/MorePlayers/issues/new?template=feature_request.md)
- 💬 [Discussions](https://github.com/Rxflex/MorePlayers/discussions)

## 📊 Project Status

This mod is actively maintained. If you encounter any issues, please report them!

---

Made with ❤️ by [Rxflex](https://github.com/Rxflex)
