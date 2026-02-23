# Dump Directory

Эта папка содержит дамп игры Pit of Goblin, созданный с помощью Il2CppDumper.

## Содержимое

- `DummyDll/` - Dummy DLL файлы для компиляции мода
- `dump.cs` - Полный дамп классов игры (не коммитится, слишком большой)
- `script.json` - Метаданные дампа (не коммитится)

## Какие DLL включены в репозиторий

Для сборки на CI/CD включены только необходимые DLL:

- ✅ `Assembly-CSharp.dll` - Основной код игры
- ✅ `UnityEngine.dll` - Unity Engine
- ✅ `UnityEngine.CoreModule.dll` - Unity Core
- ✅ `Unity.Netcode.Runtime.dll` - Сетевой код
- ✅ `Unity.Services.Lobbies.dll` - Лобби система

Остальные DLL игнорируются через `.gitignore`.

## MelonLoader DLL

MelonLoader.dll и 0Harmony.dll НЕ включены в репозиторий.

Они скачиваются автоматически:
- **Локально**: Берутся из установленной игры (`<Игра>\MelonLoader\net6\`)
- **CI/CD**: Скачиваются в workflow на шаге "Download MelonLoader"

## Как обновить дамп

Если игра обновилась:

1. Запусти Il2CppDumper на новой версии игры
2. Скопируй результаты в эту папку
3. Закоммить только нужные DLL:
   ```bash
   git add -f dump/DummyDll/Assembly-CSharp.dll
   git add -f dump/DummyDll/UnityEngine.dll
   git add -f dump/DummyDll/UnityEngine.CoreModule.dll
   git add -f dump/DummyDll/Unity.Netcode.Runtime.dll
   git add -f dump/DummyDll/Unity.Services.Lobbies.dll
   ```

## Размер файлов

Включенные DLL занимают примерно:
- Assembly-CSharp.dll: ~10-20 MB
- UnityEngine.dll: ~1-2 MB
- UnityEngine.CoreModule.dll: ~1-2 MB
- Unity.Netcode.Runtime.dll: ~500 KB
- Unity.Services.Lobbies.dll: ~200 KB

Общий размер: ~15-25 MB (приемлемо для Git)

## Примечание

Файл `dump.cs` очень большой (>50 MB) и не нужен для сборки, поэтому он исключен из репозитория.
