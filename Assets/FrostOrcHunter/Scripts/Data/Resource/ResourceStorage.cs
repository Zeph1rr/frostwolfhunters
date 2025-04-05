using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data.Resource
{
    [Serializable]
    public class ResourceStorage
    {
        [SerializeField] private List<Resource> _resources;
        public List<Resource> Resources => _resources;
        public event Action OnResourcesChanged;

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
            _resources = new List<Resource>();
            foreach(var key in keys)
            {
                Resources.Add(new Resource(key, 0));
            }
        }

        public void AddResource(string key, int value)
        {
            var resource = GetResourceByName(key);
            resource.IncreaseValue(value);
            OnResourcesChanged?.Invoke();
        }

        public void AddResources(Dictionary<string,int> resources)
        {
            foreach (var key in resources.Keys)
            {
                var resource = GetResourceByName(key);
                resource.IncreaseValue(resources[key]);
            }
            OnResourcesChanged?.Invoke();
        }

        public void AddResources(List<Resource> resources)
        {
            foreach (var resourceEntry in resources)
            {
                var resource = GetResourceByName(resourceEntry.Name);
                resource.IncreaseValue(resourceEntry.Value);
            }
            OnResourcesChanged?.Invoke();
        }

        public void SpendResource(string key, int value)
        {
            var resource = GetResourceByName(key);
            resource.DecreaseValue(value);
            OnResourcesChanged?.Invoke();
        }

        public void SpendResources(List<Resource> resources)
        {
            foreach (var resourceEntry in resources)
            {
                Resource resource = GetResourceByName(resourceEntry.Name);
                resource.DecreaseValue(resourceEntry.Value);
            }
            OnResourcesChanged?.Invoke();
        }

        public void Buy(string key, int value, Action onSuccess)
        {
            var resource = GetResourceByName(key);
            try
            {
                resource.DecreaseValue(value);
                onSuccess?.Invoke();
                OnResourcesChanged?.Invoke();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log(e);
            }
        }

        public void DecreaseResource(string key, float multiplier)
        {
            var resource = GetResourceByName(key);
            resource.DecreaseByMultiplyer(multiplier);
            OnResourcesChanged?.Invoke();
        }

        public void DecreaseAll(float multiplier)
        {
            foreach(var resource in Resources)
            {
                resource.DecreaseByMultiplyer(multiplier);
            }
            OnResourcesChanged?.Invoke();
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
            foreach(var resource in _resources)
            {
                Debug.Log($"{resource.Name}: {resource.Value}");
            }
        }
    }
}