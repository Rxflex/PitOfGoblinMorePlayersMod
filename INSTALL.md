# Installation Guide

## Step 1: Install MelonLoader

1. Download **MelonLoader.Installer.exe** from:
   https://github.com/LavaGang/MelonLoader/releases/latest

2. Run the installer

3. Click "SELECT" and choose your game folder:
   ```
   C:\Program Files (x86)\Steam\steamapps\common\Pit of Goblin
   ```
   (or wherever your game is installed)

4. Click "INSTALL"

5. Wait for installation to complete

6. Click "OK"

## Step 2: First Run

1. Launch the game

2. **Wait** - First launch takes longer (MelonLoader is setting up)

3. You should see a console window with MelonLoader logs

4. Once the game loads, close it

5. Verify these folders were created:
   ```
   <Game Folder>/
   ├── MelonLoader/
   ├── Mods/
   └── UserData/
   ```

## Step 3: Install MorePlayers Mod

1. Download `MorePlayers.dll` from:
   https://github.com/Rxflex/MorePlayers/releases/latest

2. Copy `MorePlayers.dll` to:
   ```
   <Game Folder>/Mods/MorePlayers.dll
   ```

3. Launch the game

4. Check the console - you should see:
   ```
   [MorePlayers] ========================================
   [MorePlayers] MorePlayers mod initializing...
   [MorePlayers] Max players set to: 20
   [MorePlayers] ========================================
   ```

## Step 4: Configure (Optional)

1. Close the game

2. Open:
   ```
   <Game Folder>/UserData/MelonPreferences.cfg
   ```

3. Find the `[MorePlayers]` section:
   ```ini
   [MorePlayers]
   MaxPlayers = 20
   ```

4. Change `20` to your desired number (2-100)

5. Save the file

6. Launch the game

## Troubleshooting

### MelonLoader console doesn't appear
- Make sure you installed MelonLoader correctly
- Try running the game as Administrator
- Check `MelonLoader/Latest.log` for errors

### Mod doesn't load
- Verify `MorePlayers.dll` is in `Mods/` folder (not `Plugins/`)
- Check `MelonLoader/Latest.log` for errors
- Make sure MelonLoader installed successfully

### Game crashes
- Check `MelonLoader/Latest.log` for error messages
- Try removing the mod and see if game works
- Verify game files integrity in Steam

### Max players not working
- Make sure you're the host (lobby creator)
- Verify config was saved correctly
- Check logs for "Successfully applied X/3 patches"
- All players should have the mod for best results

## Uninstallation

### Remove Mod Only
Delete `<Game Folder>/Mods/MorePlayers.dll`

### Remove MelonLoader
Delete these from game folder:
- `MelonLoader/` folder
- `Mods/` folder
- `UserData/` folder
- `version.dll`
- `dobby.dll`

## Need Help?

- Check logs: `<Game Folder>/MelonLoader/Latest.log`
- Report issues: https://github.com/Rxflex/MorePlayers/issues
- Discord: [Your Discord if you have one]
