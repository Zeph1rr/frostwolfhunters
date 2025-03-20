using FrostWolfHunters.Scripts.Game.GameRoot;

namespace FrostWolfHunters.Scripts.Game.MainMenu.Root
{
    public class MainMenuExitParams
    {
        public SceneEnterParams TargetSceneEnterParams { get; }
        
        public MainMenuExitParams(SceneEnterParams targetSceneEnterParams)
        {
            TargetSceneEnterParams = targetSceneEnterParams;
        }
    }
}