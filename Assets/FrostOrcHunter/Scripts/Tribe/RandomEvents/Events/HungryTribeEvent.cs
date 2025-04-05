using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Data.Enums;

namespace FrostOrcHunter.Scripts.Tribe.RandomEvents.Events
{
    public class HungryTribeEvent : RandomEvent
    {
        
        public HungryTribeEvent(string title, string description, string onFailureExpand, string onSuccessExpand) : base(title, description, onFailureExpand, onSuccessExpand)
        {
        }

        public override void Run(GameData gameData)
        {
            var eat = gameData.ResourceStorage.GetResourceByName(ResourceType.Eat.ToString());
            var failure = false;
            if (eat.Value == 0)
            {
                failure = true;
            }
            else
            {
                gameData.ResourceStorage.DecreaseResource(ResourceType.Eat.ToString(), 0.9f);
            }
            Draw(failure);
        }
    }
}