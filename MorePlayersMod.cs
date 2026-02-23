using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

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
        }

        // Переопределяем HarmonyInit чтобы избежать автоматического сканирования атрибутов
        public override void OnLateInitializeMelon()
        {
            // Применяем патчи вручную после полной инициализации
            ApplyPatches();
        }

        private void ApplyPatches()
        {
            try
            {
                var harmony = HarmonyInstance;
                
                // Получаем тип NetworkHandler
                var networkHandlerType = AccessTools.TypeByName("Game.Networking.NetworkHandler");
                if (networkHandlerType == null)
                {
                    LoggerInstance.Error("Failed to find NetworkHandler type!");
                    return;
                }

                // Патч для Awake
                var awakeMethod = AccessTools.Method(networkHandlerType, "Awake");
                if (awakeMethod != null)
                {
                    var awakePrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.Awake_Prefix));
                    harmony.Patch(awakeMethod, prefix: new HarmonyMethod(awakePrefix));
                    LoggerInstance.Msg("Patched NetworkHandler.Awake");
                }
                else
                {
                    LoggerInstance.Warning("NetworkHandler.Awake method not found");
                }

                // Патч для CreateLobbyAsync
                var createLobbyMethod = AccessTools.Method(networkHandlerType, "CreateLobbyAsync", new[] { typeof(string), typeof(int) });
                if (createLobbyMethod != null)
                {
                    var createLobbyPrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.CreateLobbyAsync_Prefix));
                    harmony.Patch(createLobbyMethod, prefix: new HarmonyMethod(createLobbyPrefix));
                    LoggerInstance.Msg("Patched NetworkHandler.CreateLobbyAsync");
                }
                else
                {
                    LoggerInstance.Warning("NetworkHandler.CreateLobbyAsync method not found");
                }

                // Патч для OnApprovingConnection
                var onApprovingMethod = AccessTools.Method(networkHandlerType, "OnApprovingConnection");
                if (onApprovingMethod != null)
                {
                    var onApprovingPrefix = AccessTools.Method(typeof(NetworkHandlerPatches), nameof(NetworkHandlerPatches.OnApprovingConnection_Prefix));
                    harmony.Patch(onApprovingMethod, prefix: new HarmonyMethod(onApprovingPrefix));
                    LoggerInstance.Msg("Patched NetworkHandler.OnApprovingConnection");
                }
                else
                {
                    LoggerInstance.Warning("NetworkHandler.OnApprovingConnection method not found");
                }

                LoggerInstance.Msg("All patches applied successfully!");
            }
            catch (System.Exception ex)
            {
                LoggerInstance.Error($"Failed to apply patches: {ex}");
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
