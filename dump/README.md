# Game DLLs Dump

This folder should contain the game's DLL files needed for compilation. These files are **not included in the repository** because they are:
- Large in size (>100 MB total)
- Copyrighted by the game developers
- Specific to each game installation

## How to Get These Files

### Option 1: Use Il2CppDumper (Recommended)

1. Download [Il2CppDumper](https://github.com/Perfare/Il2CppDumper/releases)
2. Run Il2CppDumper with your game files:
   - `GameAssembly.dll` from game folder
   - `global-metadata.dat` from `Pit of Goblin_Data\il2cpp_data\Metadata\`
3. Copy the generated `DummyDll` folder to this location

### Option 2: Copy from BepInEx

If you have BepInEx installed, you can copy DLLs from:
```
<Game Folder>\BepInEx\unhollowed\
```

### Required Files for Compilation

At minimum, you need:
- `Assembly-CSharp.dll` - Main game code
- `Unity.Netcode.Runtime.dll` - Networking

## Structure

After dumping, your folder should look like:
```
dump/
├── DummyDll/
│   ├── Assembly-CSharp.dll
│   ├── Unity.Netcode.Runtime.dll
│   └── ... (other DLLs)
└── README.md (this file)
```

## Note for Developers

These DLL files are only needed for **compiling** the mod. They are not distributed with the mod and are not required by end users.

## CI/CD

GitHub Actions workflow will fail without these files. The project uses NuGet for Unity DLLs, so only game-specific DLLs need to be provided manually.
