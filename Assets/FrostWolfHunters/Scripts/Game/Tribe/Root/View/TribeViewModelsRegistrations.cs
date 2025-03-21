using BaCon;

namespace FrostWolfHunters.Scripts.Game.Tribe.Root.View
{
    public static class TribeViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.RegisterFactory(c => new UITribeRootViewModel()).AsSingle();
        }
    }
}