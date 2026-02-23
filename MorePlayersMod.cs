using MelonLoader;

[assembly: MelonInfo(typeof(MorePlayers.MorePlayersMod), "MorePlayers", "1.0.0", "Rxflex")]

namespace MorePlayers
{
    public class MorePlayersMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("MorePlayers mod initialized!");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Scene initialized: {sceneName}");
        }
    }
}
