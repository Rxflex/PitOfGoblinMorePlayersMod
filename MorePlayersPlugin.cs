using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;

namespace MorePlayers
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MorePlayersPlugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        internal static ConfigEntry<int> MaxPlayersConfig;
        
        public static int MaxPlayers => MaxPlayersConfig.Value;

        private void Awake()
        {
            Logger = base.Logger;
            
            // Создаем конфиг
            MaxPlayersConfig = Config.Bind(
                "General",
                "MaxPlayers",
                20,
                new ConfigDescription(
                    "Maximum number of players allowed in a lobby (default: 20, game default: 4)",
                    new AcceptableValueRange<int>(2, 100)
                )
            );

            Logger.LogInfo($"MorePlayers v{PluginInfo.PLUGIN_VERSION} loaded!");
            Logger.LogInfo($"Max players set to: {MaxPlayers}");

            // Применяем Harmony патчи
            try
            {
                var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Logger.LogInfo("Harmony patches applied successfully!");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to apply Harmony patches: {ex}");
            }
        }
    }
}
