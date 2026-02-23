# MorePlayers Mod for Pit of Goblin

[![Build and Release](https://github.com/Rxflex/MorePlayers/actions/workflows/build.yml/badge.svg)](https://github.com/Rxflex/MorePlayers/actions/workflows/build.yml)
[![CodeQL](https://github.com/Rxflex/MorePlayers/actions/workflows/codeql.yml/badge.svg)](https://github.com/Rxflex/MorePlayers/actions/workflows/codeql.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Increases the maximum player count in Pit of Goblin from 4 to a configurable amount (default: 20).

## ✨ Features

- 🎮 Configurable maximum player count (2-100)
- ⚙️ Easy to install and configure
- 🔧 Works with BepInEx 5
- 📝 Automatic config generation
- 🛡️ Safe and tested

## 📦 Installation

### For Users

1. **Install BepInEx**
   - Download [BepInEx 5 IL2CPP](https://github.com/BepInEx/BepInEx/releases) (get the IL2CPP version)
   - Extract all files to your game folder (where `Pit of Goblin.exe` is located)
   - Run the game once to generate BepInEx folders
   - Close the game

2. **Install MorePlayers Mod**
   - Download `MorePlayers.dll` from [Releases](https://github.com/Rxflex/MorePlayers/releases)
   - Copy to `BepInEx/plugins/` folder
   - Run the game

3. **Configure (Optional)**
   - After first run, edit `BepInEx/config/com.rxflex.moreplayers.cfg`
   - Change `MaxPlayers` value (2-100)
   - Save and restart the game

## ⚙️ Configuration

The config file is located at `BepInEx/config/com.rxflex.moreplayers.cfg`

```ini
[General]
## Maximum number of players allowed in a lobby (default: 20, game default: 4)
# Setting type: Int32
# Default value: 20
# Acceptable value range: From 2 to 100
MaxPlayers = 20
```

Change `20` to any number between 2 and 100.

## 🔨 Building from Source

### Prerequisites
- [.NET SDK 6.0](https://dotnet.microsoft.com/download) or later
- Git

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

The compiled DLL will be in `bin/Release/netstandard2.1/MorePlayers.dll`

Or simply run `build.bat` on Windows.

## 📋 Requirements

- **BepInEx 5.x** (IL2CPP version)
- **Pit of Goblin** (Il2Cpp version)
- **.NET Standard 2.1** runtime (included with BepInEx)

## 🐛 Troubleshooting

### Mod doesn't load
- Verify BepInEx is installed correctly
- Check `BepInEx/LogOutput.log` for errors
- Make sure you downloaded the IL2CPP version of BepInEx

### Game crashes
- Check if game was updated (may need new mod version)
- Verify BepInEx version is compatible with your game version
- Look for error messages in `BepInEx/LogOutput.log`

### Max players not working
- Ensure config file was generated in `BepInEx/config/`
- Verify patches applied successfully (check logs)
- Make sure the host has the mod installed
- All players should have the mod for best compatibility

## 🤝 Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- BepInEx team for the modding framework
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
