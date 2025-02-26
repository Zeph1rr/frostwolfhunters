using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceStorage
{
    [SerializeField] private Dictionary<string, int> _resources = new();
    public Dictionary<string, int> Resources => _resources;

    public ResourceStorage()
    {
        foreach(var resourceType in Enum.GetNames(typeof(ResourceType)))
        {
            _resources.Add(resourceType, 0);
        }
    }

    public void Initialize(Dictionary<string, int> resources)
    {
        _resources = new();
        foreach(var resourceType in Enum.GetNames(typeof(ResourceType)))
        {
            _resources.Add(resourceType, 0);
        }
        AddResources(resources);
    }

    public void AddResources(Dictionary<string, int> resources)
    {
        foreach(string key in resources.Keys)
        {
            if(!Enum.IsDefined(typeof(ResourceType), key))
            {
                Debug.LogWarning($"Unknown resource type: {key}");
                continue;
            }
            _resources[key] += resources[key];
        }
    }

    public bool TrySpendResources(Dictionary<string, int> resources)
    {
        Dictionary<string, int>temp_resources = _resources.ToDictionary(entry => entry.Key, entry => entry.Value);
        foreach(string key in resources.Keys)
        {
            if(!Enum.IsDefined(typeof(ResourceType), key))
            {
                Debug.LogWarning($"Unknown resource type: {key}");
                return false;
            }
            if (resources[key] > temp_resources[key])
            {
                return false;
            }
        }
        SpendResources(resources);
        return true;
    }

    private void SpendResources(Dictionary<string, int> resources)
    {
        foreach(string key in resources.Keys)
        {
            _resources[key] -= resources[key];
        }
    }
}
