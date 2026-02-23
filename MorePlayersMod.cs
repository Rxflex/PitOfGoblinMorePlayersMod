using MelonLoader;
using HarmonyLib;
using System;
using System.Linq;

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
            LoggerInstance.Msg("========================================");
            LoggerInstance.Msg("MorePlayers mod initializing...");
            
            try
            {
                // Создаем категорию конфигурации
                _configCategory = MelonPreferences.CreateCategory("MorePlayers");
                _maxPlayersEntry = _configCategory.CreateEntry(
                    "MaxPlayers", 
                    MOD_DEFAULT_MAX_PLAYERS, 
                    "Maximum number of players (2-100, default: 20)"
                );

                // Загружаем значение из конфига
                MaxPlayers = _maxPlayersEntry.Value;

                // Валидация значения
                if (MaxPlayers < MIN_PLAYERS)
                {
                    LoggerInstance.Warning($"MaxPlayers {MaxPlayers} too low, setting to {MIN_PLAYERS}");
                    MaxPlayers = MIN_PLAYERS;
                    _maxPlayersEntry.Value = MIN_PLAYERS;
                }
                else if (MaxPlayers > MAX_PLAYERS_LIMIT)
                {
                    LoggerInstance.Warning($"MaxPlayers {MaxPlayers} too high, setting to {MAX_PLAYERS_LIMIT}");
                    MaxPlayers = MAX_PLAYERS_LIMIT;
                    _maxPlayersEntry.Value = MAX_PLAYERS_LIMIT;
                }

                _configCategory.SaveToFile();

                LoggerInstance.Msg($"Max players set to: {MaxPlayers}");
                LoggerInstance.Msg($"Game default: {GAME_DEFAULT_MAX_PLAYERS}");
                LoggerInstance.Msg("Config: UserData/MelonPreferences.cfg");
                LoggerInstance.Msg("========================================");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error($"Failed to initialize config: {ex}");
            }
        }

        public override void OnLateInitializeMelon()
        {
            try
            {
                LoggerInstance.Msg("Applying Harmony patches...");
                
                var harmony = new HarmonyLib.Harmony("com.rxflex.moreplayers");
                
                // Получаем Assembly-CSharp
                var assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
                
                if (assemblyCSharp == null)
                {
                    LoggerInstance.Error("Assembly-CSharp not found!");
                    return;
                }

                var networkHandlerType = assemblyCSharp.GetType("Game.Networking.NetworkHandler");
                if (networkHandlerType == null)
                {
                    LoggerInstance.Error("NetworkHandler type not found!");
                    return;
                }

                LoggerInstance.Msg($"Found NetworkHandler: {networkHandlerType.FullName}");

                int patchCount = 0;

                // Патч Awake
                var awakeMethod = AccessTools.Method(networkHandlerType, "Awake");
                if (awakeMethod != null)
                {
                    harmony.Patch(
                        awakeMethod,
                        prefix: new HarmonyMethod(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.Awake_Prefix))
                    );
                    LoggerInstance.Msg("✓ Patched NetworkHandler.Awake");
                    patchCount++;
                }

                // Патч CreateLobbyAsync
                var createLobbyMethod = AccessTools.Method(networkHandlerType, "CreateLobbyAsync", new[] { typeof(string), typeof(int) });
                if (createLobbyMethod != null)
                {
                    harmony.Patch(
                        createLobbyMethod,
                        prefix: new HarmonyMethod(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.CreateLobbyAsync_Prefix))
                    );
                    LoggerInstance.Msg("✓ Patched NetworkHandler.CreateLobbyAsync");
                    patchCount++;
                }

                // Патч OnApprovingConnection
                var onApprovingMethod = AccessTools.Method(networkHandlerType, "OnApprovingConnection");
                if (onApprovingMethod != null)
                {
                    harmony.Patch(
                        onApprovingMethod,
                        prefix: new HarmonyMethod(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.OnApprovingConnection_Prefix))
                    );
                    LoggerInstance.Msg("✓ Patched NetworkHandler.OnApprovingConnection");
                    patchCount++;
                }

                LoggerInstance.Msg("========================================");
                LoggerInstance.Msg($"Successfully applied {patchCount}/3 patches!");
                LoggerInstance.Msg("Mod is ready!");
                LoggerInstance.Msg("========================================");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error("========================================");
                LoggerInstance.Error($"CRITICAL ERROR: Failed to apply patches!");
                LoggerInstance.Error($"Exception: {ex}");
                LoggerInstance.Error("========================================");
            }
        }
    }

    public static class NetworkHandlerPatches
    {
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
            catch (Exception ex)
            {
                MelonLogger.Error($"[NetworkHandler.Awake] Error: {ex}");
            }
        }

        public static void CreateLobbyAsync_Prefix(ref int maxPlayers)
        {
            MelonLogger.Msg($"[NetworkHandler.CreateLobbyAsync] Original: {maxPlayers}, changing to: {MorePlayersMod.MaxPlayers}");
            maxPlayers = MorePlayersMod.MaxPlayers;
        }

        public static void OnApprovingConnection_Prefix()
        {
            MelonLogger.Msg($"[NetworkHandler.OnApprovingConnection] Client connecting (Max: {MorePlayersMod.MaxPlayers})");
        }
    }
}
