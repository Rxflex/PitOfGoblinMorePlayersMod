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

### Option 2: Copy from MelonLoader

After installing MelonLoader, you can copy DLLs from:
```
<Game Folder>\MelonLoader\Il2CppAssemblies\
```

### Required Files for Compilation

You need these DLLs:
- `MelonLoader.dll` - From `<Game Folder>\MelonLoader\net6\`
- `0Harmony.dll` - From `<Game Folder>\MelonLoader\net6\`
- `Assembly-CSharp.dll` - Main game code
- `Unity.Netcode.Runtime.dll` - Networking
- `UnityEngine.dll` - Unity engine
- `UnityEngine.CoreModule.dll` - Unity core

## Structure

After setup, your folder should look like:
```
dump/
├── DummyDll/
│   ├── MelonLoader.dll
│   ├── 0Harmony.dll
│   ├── Assembly-CSharp.dll
│   ├── Unity.Netcode.Runtime.dll
│   ├── UnityEngine.dll
│   ├── UnityEngine.CoreModule.dll
│   └── ... (other DLLs)
└── README.md (this file)
```

## Note for Developers

These DLL files are only needed for **compiling** the mod. They are not distributed with the mod and are not required by end users.

## CI/CD

GitHub Actions workflow will fail without these files. For local development:

1. Install MelonLoader in your game
2. Copy required DLLs from game folder to `dump/DummyDll/`
3. Build the project

The DLLs are not committed to the repository due to size and copyright.
