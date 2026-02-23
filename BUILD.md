# Инструкция по сборке MorePlayers

## Требования для сборки

1. .NET 6.0 SDK или выше
   - Скачать: https://dotnet.microsoft.com/download/dotnet/6.0

2. Visual Studio 2022 или Rider (опционально)

3. Дамп игры (уже включен в папку `dump/DummyDll/`)

## Шаги сборки

### Вариант 1: Командная строка

1. Откройте командную строку в папке проекта
2. Выполните команду:
   ```bash
   dotnet build -c Release
   ```
3. Скомпилированный мод будет находиться в:
   ```
   bin\Release\net6.0\MorePlayers.dll
   ```

### Вариант 2: Visual Studio

1. Откройте `MorePlayers.csproj` в Visual Studio
2. Выберите конфигурацию `Release`
3. Нажмите `Build` -> `Build Solution` (или F6)
4. DLL будет в `bin\Release\net6.0\`

### Вариант 3: Rider

1. Откройте проект в JetBrains Rider
2. Выберите конфигурацию `Release`
3. Нажмите `Build` -> `Build Solution`
4. DLL будет в `bin\Release\net6.0\`

## Установка мода

1. Установите MelonLoader в папку с игрой Pit of Goblin:
   - Скачайте MelonLoader: https://github.com/LavaGang/MelonLoader/releases
   - Запустите установщик и выберите папку с игрой

2. Скопируйте `MorePlayers.dll` в папку:
   ```
   <Папка игры>\Mods\
   ```

3. Запустите игру через `Pit of Goblin.exe`

## Настройка

После первого запуска игры с модом будет создан файл конфигурации:
```
<Папка игры>\UserData\MelonPreferences.cfg
```

Найдите секцию `[MorePlayers]` и измените значение:
```ini
[MorePlayers]
MaxPlayers = 8
```

Допустимые значения: от 2 до 16

## Проверка работы мода

1. Запустите игру
2. В консоли MelonLoader должны появиться сообщения:
   ```
   ========================================
   MorePlayers mod initialized!
   Max players set to: 8
   Default game limit: 4
   ========================================
   ```

3. При создании лобби в логах должно быть:
   ```
   [NetworkHandler.CreateLobbyAsync] Changed maxPlayers to: 8
   ```

## Решение проблем

### Мод не загружается
- Убедитесь, что MelonLoader установлен правильно
- Проверьте, что DLL находится в папке `Mods`
- Проверьте логи в `<Папка игры>\MelonLoader\Latest.log`

### Ошибки при сборке
- Убедитесь, что установлен .NET 6.0 SDK
- Проверьте, что все DLL из `dump/DummyDll/` на месте
- Попробуйте выполнить `dotnet restore` перед сборкой

### Игра крашится
- Попробуйте уменьшить значение `MaxPlayers` в конфиге
- Убедитесь, что все игроки используют одинаковую версию мода
- Проверьте логи MelonLoader на наличие ошибок

## Дополнительная информация

- GitHub: https://github.com/Rxflex
- MelonLoader Wiki: https://melonwiki.xyz/
