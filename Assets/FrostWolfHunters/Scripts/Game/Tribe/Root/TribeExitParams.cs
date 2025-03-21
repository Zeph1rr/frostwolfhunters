using FrostWolfHunters.Scripts.Game.GameRoot;

namespace FrostWolfHunters.Scripts.Game.Tribe.Root
{
    public class TribeExitParams
    {
        public SceneEnterParams TargetSceneEnterParams { get; }
        
        public TribeExitParams(SceneEnterParams targetSceneEnterParams)
        {
            TargetSceneEnterParams = targetSceneEnterParams;
        }
    }
}