using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceStorage
{
    [SerializeField] private Dictionary<string, int> _resources = new();
    public Dictionary<string, int> Resources => _resources;
    public event EventHandler OnResourcesChanged;

    public ResourceStorage()
    {
        InitializeEmptyStorage();
    }

    public void Initialize(Dictionary<string, int> resources)
    {
        InitializeEmptyStorage();
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
        OnResourcesChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ResetHuntResourceStorage()
    {
        InitializeEmptyStorage();
    }

    public void AddResource(string key, int value)
    {
        if(!Enum.IsDefined(typeof(ResourceType), key))
        {
            Debug.LogWarning($"Unknown resource type: {key}");
            return;
        }
        _resources[key] += value;
        OnResourcesChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseAll(float multyiplier)
    {
        foreach(string key in _resources.Keys.ToList())
        {
            if(!Enum.IsDefined(typeof(ResourceType), key))
            {
                Debug.LogWarning($"Unknown resource type: {key}");
                continue;
            }
            if (_resources[key] == 0) return;
            _resources[key] = Mathf.FloorToInt(_resources[key] * multyiplier);
        }
    }

    public bool TrySpendResource(string key, int value)
    {
        if(!Enum.IsDefined(typeof(ResourceType), key))
        {
            Debug.LogWarning($"Unknown resource type: {key}");
            return false;
        }
        if (value > _resources[key]) return false;
        SpendResource(key, value);
        return true;
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

    public void PrintResources()
    {
        foreach(string key in _resources.Keys)
        {
            Debug.Log($"{key}: {_resources[key]}");
        }
    }

    private void SpendResources(Dictionary<string, int> resources)
    {
        foreach(string key in resources.Keys)
        {
            _resources[key] -= resources[key];
        }
        OnResourcesChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SpendResource(string key, int value)
    {
        _resources[key] -= value;
        OnResourcesChanged?.Invoke(this, EventArgs.Empty);
    }

    private void InitializeEmptyStorage()
    {
        _resources = new();
        foreach(var resourceType in Enum.GetNames(typeof(ResourceType)))
        {
            _resources.Add(resourceType, 0);
        }
    }
}
