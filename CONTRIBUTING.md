# Contributing to MorePlayers

Thank you for your interest in contributing! Here are some guidelines to help you get started.

## Development Setup

1. **Prerequisites**
   - .NET SDK 6.0 or later
   - Git
   - A text editor or IDE (Visual Studio, VS Code, Rider)

2. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/MorePlayers.git
   cd MorePlayers
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the project**
   ```bash
   dotnet build -c Release
   ```

## Project Structure

```
MorePlayers/
├── MorePlayersPlugin.cs           # Main plugin entry point
├── PluginInfo.cs                  # Plugin metadata
├── Patches/
│   └── NetworkHandlerPatches.cs   # Harmony patches
├── dump/                          # Game DLLs (not in repo)
│   └── DummyDll/
│       └── Assembly-CSharp.dll
└── MorePlayers.csproj             # Project configuration
```

## Making Changes

1. **Create a branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes**
   - Follow existing code style
   - Add comments for complex logic
   - Test your changes in-game

3. **Test thoroughly**
   - Build the mod
   - Copy to `BepInEx/plugins/`
   - Test in-game with different player counts
   - Check logs for errors

4. **Commit your changes**
   ```bash
   git add .
   git commit -m "Add: description of your changes"
   ```

5. **Push and create a Pull Request**
   ```bash
   git push origin feature/your-feature-name
   ```

## Code Style

- Use meaningful variable names
- Add XML documentation comments for public methods
- Keep methods focused and small
- Use try-catch blocks for error handling
- Log important events and errors

## Testing Checklist

Before submitting a PR, ensure:
- [ ] Code compiles without errors or warnings
- [ ] Mod loads in-game without crashes
- [ ] Config file generates correctly
- [ ] Config changes are applied
- [ ] Harmony patches apply successfully
- [ ] Logs show expected behavior
- [ ] Tested with 2, 4, 10, and 20+ players

## Reporting Bugs

Use the GitHub issue tracker with the bug report template. Include:
- Game version
- BepInEx version
- Mod version
- Steps to reproduce
- Log files (`BepInEx/LogOutput.log`)

## Suggesting Features

Use the GitHub issue tracker with the feature request template. Describe:
- The problem you're trying to solve
- Your proposed solution
- Any alternatives you've considered

## Questions?

Feel free to open an issue for questions or join discussions!

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
