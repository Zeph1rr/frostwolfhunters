using FrostWolfHunters.Scripts.Game.GameRoot;

namespace FrostWolfHunters.Scripts.Game.Tribe.Root
{
    public class TribeEnterParams : SceneEnterParams
    {
        public int MapId { get; }

        public TribeEnterParams(int mapId) : base(Scenes.TRIBE)
        {
            MapId = mapId;
        }
    }
}