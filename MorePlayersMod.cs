using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Linq;

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

        private static MelonPreferences_Category _configCategory;
        private static MelonPreferences_Entry<int> _maxPlayersEntry;

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
                LoggerInstance.Msg("[OnInitializeMelon] Waiting for game to start...");
                LoggerInstance.Msg("[OnInitializeMelon] If game crashes after this message, it's a game/MelonLoader issue, not the mod");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnInitializeMelon] Error during initialization: {ex}");
            }
        }

        private bool _patchesApplied = false;
        private int _updateCount = 0;
        private bool _firstUpdateLogged = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                LoggerInstance.Msg($"[OnSceneWasLoaded] Scene loaded: {sceneName} (index: {buildIndex})");
                
                // Применяем патчи только один раз при загрузке первой сцены
                if (!_patchesApplied)
                {
                    LoggerInstance.Msg("[OnSceneWasLoaded] Will apply patches in 60 frames...");
                }
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnSceneWasLoaded] Error: {ex}");
            }
        }

        public override void OnUpdate()
        {
            try
            {
                // Логируем первый Update
                if (!_firstUpdateLogged)
                {
                    LoggerInstance.Msg("[OnUpdate] First update called - game is running!");
                    _firstUpdateLogged = true;
                }
                
                // Применяем патчи после 60 кадров (примерно 1 секунда) если сцена не загрузилась
                if (!_patchesApplied)
                {
                    _updateCount++;
                    
                    if (_updateCount == 60)
                    {
                        LoggerInstance.Msg("[OnUpdate] 60 frames passed, applying patches...");
                        ApplyPatches();
                        _patchesApplied = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"[OnUpdate] Error: {ex}");
            }
        }

        private void ApplyPatches()
        {
            try
            {
                LoggerInstance.Msg("[ApplyPatches] Starting to apply Harmony patches...");
                
                var harmony = HarmonyInstance;
                LoggerInstance.Msg($"[ApplyPatches] Harmony instance ID: {harmony.Id}");
                
                // Получаем тип NetworkHandler через Assembly-CSharp
                LoggerInstance.Msg("[ApplyPatches] Looking for Game.Networking.NetworkHandler type...");
                
                // Для Il2Cpp нужно искать тип в конкретной сборке
                var assemblyCSharp = System.AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
                
                if (assemblyCSharp == null)
                {
                    LoggerInstance.Error("[ApplyPatches] Assembly-CSharp not found!");
                    return;
                }
                
                LoggerInstance.Msg($"[ApplyPatches] Found Assembly-CSharp: {assemblyCSharp.FullName}");
                
                var networkHandlerType = assemblyCSharp.GetType("Game.Networking.NetworkHandler");
                if (networkHandlerType == null)
                {
                    LoggerInstance.Error("[ApplyPatches] Failed to find NetworkHandler type!");
                    LoggerInstance.Error("[ApplyPatches] The mod will not work. Please check if the game was updated.");
                    return;
                }
                LoggerInstance.Msg($"[ApplyPatches] Found NetworkHandler type: {networkHandlerType.FullName}");

                // Патч для Awake
                LoggerInstance.Msg("[ApplyPatches] Patching NetworkHandler.Awake...");
                var awakeMethod = AccessTools.Method(networkHandlerType, "Awake");
                if (awakeMethod != null)
                {
                    var awakePrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.Awake_Prefix));
                    harmony.Patch(awakeMethod, prefix: new HarmonyMethod(awakePrefix));
                    LoggerInstance.Msg("[ApplyPatches] ✓ Patched NetworkHandler.Awake");
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
                }
                else
                {
                    LoggerInstance.Warning("[ApplyPatches] NetworkHandler.OnApprovingConnection method not found");
                }

                LoggerInstance.Msg("========================================");
                LoggerInstance.Msg("[ApplyPatches] All patches applied successfully!");
                LoggerInstance.Msg("========================================");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error("========================================");
                LoggerInstance.Error($"[ApplyPatches] CRITICAL ERROR: Failed to apply patches!");
                LoggerInstance.Error($"[ApplyPatches] Exception: {ex}");
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
