using System.Collections.Generic;
using Zeph1rr.Core.Resources;

[System.Serializable]
public class ResourcesSerialazable
{
    public int Fur; 
    public int Eat;
    public int Stone;
    public int Bones;
    public int Wood;


    public ResourcesSerialazable(ResourceStorage resourceStorage)
    {
        Fur = resourceStorage.GetResourceValueByName(ResourceType.Fur.ToString());
        Eat = resourceStorage.GetResourceValueByName(ResourceType.Eat.ToString());
        Stone = resourceStorage.GetResourceValueByName(ResourceType.Stone.ToString());
        Bones = resourceStorage.GetResourceValueByName(ResourceType.Bones.ToString());
        Wood = resourceStorage.GetResourceValueByName(ResourceType.Wood.ToString());
    }

    public ResourceStorage ToResourceStorage()
    {
        Dictionary<string, int> resources = new()
        {
            {ResourceType.Fur.ToString(), Fur},
            {ResourceType.Eat.ToString(), Eat},
            {ResourceType.Stone.ToString(), Stone},
            {ResourceType.Bones.ToString(), Bones},
            {ResourceType.Wood.ToString(), Wood},
        };
        ResourceStorage resouceStorage = new(resources);
        return resouceStorage;
    }
}
