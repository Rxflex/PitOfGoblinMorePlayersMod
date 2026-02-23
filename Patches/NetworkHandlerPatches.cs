using HarmonyLib;
using System;

namespace MorePlayers.Patches
{
    [HarmonyPatch]
    internal class NetworkHandlerPatches
    {
        // Патч для метода Awake - устанавливаем максимальное количество клиентов
        [HarmonyPatch(typeof(Game.Networking.NetworkHandler), "Awake")]
        [HarmonyPrefix]
        private static void Awake_Prefix(Game.Networking.NetworkHandler __instance)
        {
            try
            {
                var field = AccessTools.Field(typeof(Game.Networking.NetworkHandler), "m_maxClientCount");
                if (field != null)
                {
                    field.SetValue(__instance, MorePlayersPlugin.MaxPlayers);
                    MorePlayersPlugin.Logger.LogInfo($"Set m_maxClientCount to {MorePlayersPlugin.MaxPlayers}");
                }
                else
                {
                    MorePlayersPlugin.Logger.LogWarning("Field m_maxClientCount not found!");
                }
            }
            catch (Exception ex)
            {
                MorePlayersPlugin.Logger.LogError($"Failed to patch m_maxClientCount: {ex}");
            }
        }

        // Патч для CreateLobbyAsync - изменяем maxPlayers при создании лобби
        [HarmonyPatch(typeof(Game.Networking.NetworkHandler), "CreateLobbyAsync")]
        [HarmonyPrefix]
        private static void CreateLobbyAsync_Prefix(ref int maxPlayers)
        {
            MorePlayersPlugin.Logger.LogInfo($"CreateLobbyAsync called with maxPlayers: {maxPlayers}");
            maxPlayers = MorePlayersPlugin.MaxPlayers;
            MorePlayersPlugin.Logger.LogInfo($"Changed maxPlayers to: {maxPlayers}");
        }

        // Патч для OnApprovingConnection - логируем подключения
        [HarmonyPatch(typeof(Game.Networking.NetworkHandler), "OnApprovingConnection")]
        [HarmonyPrefix]
        private static void OnApprovingConnection_Prefix()
        {
            MorePlayersPlugin.Logger.LogInfo($"Client attempting to connect (Max: {MorePlayersPlugin.MaxPlayers})");
        }
    }
}
