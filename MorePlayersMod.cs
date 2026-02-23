using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Harmony = HarmonyLib.Harmony;

[assembly: MelonInfo(typeof(MorePlayers.MorePlayersMod), "MorePlayers", "1.0.0", "Rxflex", "https://github.com/Rxflex")]
[assembly: MelonGame("Friendly Foe", "Pit of Goblin")]

namespace MorePlayers
{
    public class MorePlayersMod : MelonMod
    {
        private const int GAME_DEFAULT_MAX_PLAYERS = 4;
        private const int MOD_DEFAULT_MAX_PLAYERS = 20;
        private const int MIN_PLAYERS = 2;
        private const int MAX_PLAYERS_LIMIT = 100;

        private static MelonPreferences_Category? _configCategory;
        private static MelonPreferences_Entry<int>? _maxPlayersEntry;

        public static int MaxPlayers { get; private set; } = MOD_DEFAULT_MAX_PLAYERS;

        public override void OnInitializeMelon()
        {
            try
            {
                LoggerInstance.Msg("[OnInitializeMelon] Starting initialization...");
                
                // Создаем категорию конфигурации с дефолтным значением 20
                _configCategory = MelonPreferences.CreateCategory("MorePlayers");
                _maxPlayersEntry = _configCategory.CreateEntry(
                    "MaxPlayers", 
                    MOD_DEFAULT_MAX_PLAYERS, 
                    "Maximum number of players (default: 20, can be changed to any value like 6, 8, 12, etc.)"
                );

                // Загружаем значение из конфига
                MaxPlayers = _maxPlayersEntry.Value;

                // Валидация значения
                if (MaxPlayers < MIN_PLAYERS)
                {
                    LoggerInstance.Warning($"MaxPlayers value {MaxPlayers} is too low. Setting to minimum: {MIN_PLAYERS}");
                    MaxPlayers = MIN_PLAYERS;
                    _maxPlayersEntry.Value = MIN_PLAYERS;
                }
                else if (MaxPlayers > MAX_PLAYERS_LIMIT)
                {
                    LoggerInstance.Warning($"MaxPlayers value {MaxPlayers} is too high. Setting to maximum: {MAX_PLAYERS_LIMIT}");
                    MaxPlayers = MAX_PLAYERS_LIMIT;
                    _maxPlayersEntry.Value = MAX_PLAYERS_LIMIT;
                }

                _configCategory.SaveToFile();

                LoggerInstance.Msg("========================================");
                LoggerInstance.Msg("MorePlayers mod initialized!");
                LoggerInstance.Msg($"Max players set to: {MaxPlayers}");
                LoggerInstance.Msg($"Game default limit: {GAME_DEFAULT_MAX_PLAYERS}");
                LoggerInstance.Msg($"You can change MaxPlayers in the config file");
                LoggerInstance.Msg("========================================");
                
                LoggerInstance.Msg("[OnInitializeMelon] Initialization complete");
                LoggerInstance.Msg("[OnInitializeMelon] Waiting for game to fully load before applying patches...");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnInitializeMelon] Error during initialization: {ex}");
            }
        }

        private bool _patchesApplied = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                LoggerInstance.Msg($"[OnSceneWasLoaded] Scene loaded: {sceneName} (index: {buildIndex})");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnSceneWasLoaded] Error: {ex}");
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            try
            {
                LoggerInstance.Msg($"[OnSceneWasInitialized] Scene initialized: {sceneName} (index: {buildIndex})");
                
                // Применяем патчи только один раз при инициализации первой сцены
                if (!_patchesApplied)
                {
                    LoggerInstance.Msg("[OnSceneWasInitialized] Applying patches now that scene is initialized...");
                    ApplyPatches();
                    _patchesApplied = true;
                }
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnSceneWasInitialized] Error: {ex}");
            }
        }

        private void ApplyPatches()
        {
            try
            {
                LoggerInstance.Msg("[ApplyPatches] Starting to apply Harmony patches...");
                
                // Создаём новый экземпляр Harmony
                var harmony = new Harmony("com.rxflex.moreplayers");
                LoggerInstance.Msg($"[ApplyPatches] Created Harmony instance: {harmony.Id}");
                
                // Получаем тип NetworkHandler через Assembly-CSharp
                LoggerInstance.Msg("[ApplyPatches] Looking for Game.Networking.NetworkHandler type...");
                
                // Для Il2Cpp нужно искать тип в конкретной сборке
                var assemblyCSharp = System.AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
                
                if (assemblyCSharp == null)
                {
                    LoggerInstance.Error("[ApplyPatches] Assembly-CSharp not found! Game may not be fully loaded yet.");
                    LoggerInstance.Error("[ApplyPatches] Mod will not work. Try restarting the game.");
                    return;
                }
                
                LoggerInstance.Msg($"[ApplyPatches] Found Assembly-CSharp: {assemblyCSharp.FullName}");
                
                var networkHandlerType = assemblyCSharp.GetType("Game.Networking.NetworkHandler");
                if (networkHandlerType == null)
                {
                    LoggerInstance.Error("[ApplyPatches] Failed to find NetworkHandler type!");
                    LoggerInstance.Error("[ApplyPatches] The mod will not work. Please check if the game was updated.");
                    LoggerInstance.Error("[ApplyPatches] Available types in Assembly-CSharp:");
                    
                    // Выводим первые 20 типов для отладки
                    var types = assemblyCSharp.GetTypes().Take(20);
                    foreach (var t in types)
                    {
                        LoggerInstance.Error($"  - {t.FullName}");
                    }
                    return;
                }
                LoggerInstance.Msg($"[ApplyPatches] Found NetworkHandler type: {networkHandlerType.FullName}");

                int patchedCount = 0;

                // Патч для Awake
                LoggerInstance.Msg("[ApplyPatches] Patching NetworkHandler.Awake...");
                var awakeMethod = AccessTools.Method(networkHandlerType, "Awake");
                if (awakeMethod != null)
                {
                    var awakePrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.Awake_Prefix));
                    harmony.Patch(awakeMethod, prefix: new HarmonyMethod(awakePrefix));
                    LoggerInstance.Msg("[ApplyPatches] ✓ Patched NetworkHandler.Awake");
                    patchedCount++;
                }
                else
                {
                    LoggerInstance.Warning("[ApplyPatches] NetworkHandler.Awake method not found");
                }

                // Патч для CreateLobbyAsync
                LoggerInstance.Msg("[ApplyPatches] Patching NetworkHandler.CreateLobbyAsync...");
                var createLobbyMethod = AccessTools.Method(networkHandlerType, "CreateLobbyAsync", new[] { typeof(string), typeof(int) });
                if (createLobbyMethod != null)
                {
                    var createLobbyPrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.CreateLobbyAsync_Prefix));
                    harmony.Patch(createLobbyMethod, prefix: new HarmonyMethod(createLobbyPrefix));
                    LoggerInstance.Msg("[ApplyPatches] ✓ Patched NetworkHandler.CreateLobbyAsync");
                    patchedCount++;
                }
                else
                {
                    LoggerInstance.Warning("[ApplyPatches] NetworkHandler.CreateLobbyAsync method not found");
                }

                // Патч для OnApprovingConnection
                LoggerInstance.Msg("[ApplyPatches] Patching NetworkHandler.OnApprovingConnection...");
                var onApprovingMethod = AccessTools.Method(networkHandlerType, "OnApprovingConnection");
                if (onApprovingMethod != null)
                {
                    var onApprovingPrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.OnApprovingConnection_Prefix));
                    harmony.Patch(onApprovingMethod, prefix: new HarmonyMethod(onApprovingPrefix));
                    LoggerInstance.Msg("[ApplyPatches] ✓ Patched NetworkHandler.OnApprovingConnection");
                    patchedCount++;
                }
                else
                {
                    LoggerInstance.Warning("[ApplyPatches] NetworkHandler.OnApprovingConnection method not found");
                }

                LoggerInstance.Msg("========================================");
                LoggerInstance.Msg($"[ApplyPatches] Successfully applied {patchedCount}/3 patches!");
                if (patchedCount < 3)
                {
                    LoggerInstance.Warning("[ApplyPatches] Some patches were not applied. Mod may not work correctly.");
                }
                LoggerInstance.Msg("========================================");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error("========================================");
                LoggerInstance.Error($"[ApplyPatches] CRITICAL ERROR: Failed to apply patches!");
                LoggerInstance.Error($"[ApplyPatches] Exception: {ex}");
                LoggerInstance.Error($"[ApplyPatches] Stack trace: {ex.StackTrace}");
                LoggerInstance.Error("========================================");
            }
        }
    }

    public static class NetworkHandlerPatches
    {
        // Патч для увеличения максимального количества клиентов
        public static void Awake_Prefix(object __instance)
        {
            try
            {
                var field = AccessTools.Field(__instance.GetType(), "m_maxClientCount");
                if (field != null)
                {
                    field.SetValue(__instance, MorePlayersMod.MaxPlayers);
                    MelonLogger.Msg($"[NetworkHandler.Awake] Set m_maxClientCount to {MorePlayersMod.MaxPlayers}");
                }
                else
                {
                    MelonLogger.Error("[NetworkHandler.Awake] Field m_maxClientCount not found!");
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[NetworkHandler.Awake] Failed to patch m_maxClientCount: {ex}");
            }
        }

        // Патч для CreateLobbyAsync - увеличиваем maxPlayers при создании лобби
        public static void CreateLobbyAsync_Prefix(ref int maxPlayers)
        {
            MelonLogger.Msg($"[NetworkHandler.CreateLobbyAsync] Original maxPlayers: {maxPlayers}");
            maxPlayers = MorePlayersMod.MaxPlayers;
            MelonLogger.Msg($"[NetworkHandler.CreateLobbyAsync] Changed maxPlayers to: {maxPlayers}");
        }

        // Патч для проверки подключения клиентов
        public static void OnApprovingConnection_Prefix()
        {
            MelonLogger.Msg($"[NetworkHandler.OnApprovingConnection] Client attempting to connect (Max: {MorePlayersMod.MaxPlayers})");
        }
    }
}
