using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeph1rr.Core.Resources
{
    [Serializable]
    public class ResourceStorage
    {
        [SerializeField] private List<Resource> _resources;
        public List<Resource> Resources => _resources;
        public event EventHandler OnResourcesChanged;

        public ResourceStorage(string[] keys)
        {
            ResetResourceStorage(keys);
        }

        public ResourceStorage(Dictionary<string, int> resources)
        {
            ResetResourceStorage(resources.Keys.ToArray<string>());
            AddResources(resources);
        }

        public void ResetResourceStorage(string[] keys)
        {
            _resources = new();
            foreach(string key in keys)
            {
                Resources.Add(new Resource(key, 0));
            }
        }

        public void AddResource(string key, int value)
        {
            Resource resource = GetResourceByName(key);
            resource.IncreaseValue(value);
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddResources(Dictionary<string,int> resources)
        {
            foreach (string key in resources.Keys)
            {
                Resource resource = GetResourceByName(key);
                resource.IncreaseValue(resources[key]);
            }
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddResources(List<Resource> resources)
        {
            foreach (Resource resourceEntry in resources)
            {
                Resource resource = GetResourceByName(resourceEntry.Name);
                resource.IncreaseValue(resourceEntry.Value);
            }
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SpendResource(string key, int value)
        {
            Resource resource = GetResourceByName(key);
            resource.DecreaseValue(value);
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SpendResources(List<Resource> resources)
        {
            foreach (Resource resourceEntry in resources)
            {
                Resource resource = GetResourceByName(resourceEntry.Name);
                resource.DecreaseValue(resourceEntry.Value);
            }
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DecreaseAll(float multyiplier)
        {
            foreach(Resource resource in Resources)
            {
                resource.DecreaseByMultiplyer(multyiplier);
            }
            OnResourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public Resource GetResourceByName(string name)
        {
            return _resources.Find(entry => entry.Name == name) ?? throw new KeyNotFoundException($"Cannot find recource with name: {name}");;
        }

        public int GetResourceValueByName(string name)
        {
            return GetResourceByName(name).Value;
        }

        public void PrintResources()
        {
            foreach(Resource resource in _resources)
            {
                Debug.Log($"{resource.Name}: {resource.Value}");
            }
        }
    }

}
